using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMvvmToolkit.Common;
using UnityMvvmToolkit.Core.Interfaces;

namespace UnityMvvmToolkit.UITK
{
    public class VisualElementView<TBindingContext> : MonoBehaviourView<TBindingContext>
        where TBindingContext : class, INotifyPropertyChanged
    {
        private VisualElement rootVisualElement;
        public VisualElement RootVisualElement
        {
            set => rootVisualElement = value;
            get
            {
                // HACK: guard against use before initialization
                if (rootVisualElement == null)
                {
                    Init();
                }

                return rootVisualElement;
            }
        }

        protected override void OnInit()
        {
            // HACK: guard against use before initialization
            if (rootVisualElement == null)
            {
                rootVisualElement = GetRootVisualElement();
            }
        }

        /// <summary>
        /// Called during Initialization to get/resolve the root visual element
        /// used when querying for bindable UI elements.
        /// </summary>
        /// <returns>The VisualElement instance to bind to.</returns>
        protected virtual VisualElement GetRootVisualElement()
        {
            return GetComponent<UIDocument>().rootVisualElement;
        }

        protected override IBindableElementsFactory GetBindableElementsFactory()
        {
            return new BindableElementsFactory();
        }

        protected override IEnumerable<IBindableUIElement> GetBindableUIElements()
        {
            return RootVisualElement
                .Query<VisualElement>()
                .Where(element => element is IBindableUIElement)
                .Build()
                .Cast<IBindableUIElement>();
        }
    }
}