﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Core.SDK.Composite.Common
{
    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate _delegate;
        private readonly WeakReference _weakReference;
        private readonly MethodInfo _method;
        private readonly Type _delegateType;
       
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
                throw new ArgumentNullException("delegate");

            if (keepReferenceAlive)
            {
                this._delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.Method;
                _delegateType = @delegate.GetType();
            }
        }

        public Delegate Target
        {
            get
            {
                if (_delegate != null)
                {
                    return _delegate;
                }
                else
                {
                    return TryGetDelegate();
                }
            }
        }

        private Delegate TryGetDelegate()
        {
            if (_method.IsStatic)
            {
                return Delegate.CreateDelegate(_delegateType, null, _method);
            }
            object target = _weakReference.Target;
            if (target != null)
            {
                return Delegate.CreateDelegate(_delegateType, target, _method);
            }
            return null;
        }
    }
}
