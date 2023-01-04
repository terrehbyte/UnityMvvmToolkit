using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityMvvmToolkit.Core;
using UnityMvvmToolkit.Core.Interfaces;

namespace UnityMvvmToolkit.Common
{
    [DefaultExecutionOrder(1)]
    public abstract class MonoBehaviourView<TBindingContext> : MonoBehaviour
        where TBindingContext : class, INotifyPropertyChanged
    {
        protected View<TBindingContext> view;

        public TBindingContext BindingContext => view.BindingContext;

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void OnEnable()
        {
            Debug.Assert(view != null, "Cannot enable bindings on view if view is null");
            view.EnableBinding();
        }

        protected virtual void OnDisable()
        {
            Debug.Assert(view != null, "Cannot disable bindings on view if view is null");
            view.DisableBinding();
        }

        protected virtual void OnDestroy()
        {
            view.Dispose();
        }

        /// <summary>
        /// <p>
        /// One-time initialization of the view and its bindings.
        /// </p>
        /// <p>
        /// <b>Automatically called in Awake, unless overriden in derived classes.</b>
        /// </p>
        /// <p>
        /// Should be called before before enabling or disabling any
        /// data-binding operations.
        /// </p>
        /// </summary>
        /// <remarks>
        /// This prepares the view for use, creating any underlying objects
        /// such as the view implementation and viewmodel instance before
        /// setting up any data bindings.
        /// </remarks>
        protected void Init()
        {
            view = CreateView(GetBindingContext(), GetBindableElementsFactory());

            Debug.Assert(view != null, "Failed to create view.");
            
            OnInit();
            BindElements();
        }

        protected abstract void OnInit();
        protected abstract IBindableElementsFactory GetBindableElementsFactory();
        protected abstract IEnumerable<IBindableUIElement> GetBindableUIElements();

        protected virtual TBindingContext GetBindingContext()
        {
            if (typeof(TBindingContext).GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException(
                    $"Cannot create an instance of the type parameter {typeof(TBindingContext)} because it does not have a parameterless constructor.");
            }

            return Activator.CreateInstance<TBindingContext>();
        }

        protected virtual IValueConverter[] GetValueConverters()
        {
            return Array.Empty<IValueConverter>();
        }

        protected virtual IObjectProvider GetObjectProvider(TBindingContext bindingContext, IValueConverter[] converters)
        {
            return new BindingContextObjectProvider<TBindingContext>(bindingContext, converters);
        }

        private View<TBindingContext> CreateView(TBindingContext bindingContext,
            IBindableElementsFactory bindableElementsFactory)
        {
            return new View<TBindingContext>()
                .Configure(bindingContext, GetObjectProvider(bindingContext, GetValueConverters()),
                    bindableElementsFactory);
        }

        private void BindElements()
        {
            foreach (var bindableUIElement in GetBindableUIElements())
            {
                view.RegisterBindableElement(bindableUIElement, true);
            }
        }
    }
}
