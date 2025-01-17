﻿using System.ComponentModel;
using Interfaces;
using UnityEngine;
using UnityMvvmToolkit.Core.Interfaces;
using UnityMvvmToolkit.UITK;

namespace Views
{
    public class BaseView<TBindingContext> : DocumentView<TBindingContext>
        where TBindingContext : class, INotifyPropertyChanged
    {
        [SerializeField] private AppContext _appContext;

        protected IAppContext AppContext => _appContext;
        
        protected override TBindingContext GetBindingContext()
        {
            return _appContext.Resolve<TBindingContext>();
        }

        protected override IValueConverter[] GetValueConverters()
        {
            return _appContext.Resolve<IValueConverter[]>();
        }

        protected override IBindableElementsFactory GetBindableElementsFactory()
        {
            return _appContext.Resolve<IBindableElementsFactory>();
        }
    }
}