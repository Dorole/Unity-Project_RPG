using System;
using UnityEngine;

namespace RPG.Utils
{
    /// <summary>
    /// Container class. Wraps a value and ensures initialisation is
    /// called just before first use.
    /// </summary>
    public class LazyValue<T>
    {
        public delegate T InitializerDelegate();
        
        T _value;
        bool _initialized = false;
        InitializerDelegate _initializer;

        /// <summary>
        /// Sets up the container but don't initialise the value yet.
        /// </summary>
        /// <param name="initializer">
        /// The initializer delegate to call when LazyValue has not been set 
        /// for the first time. The value cannot be read without a valid initializer.
        /// </param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// Get or set the contents of the container.
        /// </summary>
        public T value
        {
            get
            {
                ForceInitialization();
                return _value;
            }
            set
            {
                _initialized = true;
                _value = value;
            }
        }

        /// <summary>
        /// Forces the initialization of the value via the delegate.
        /// Ensures _value has at a bare minimum the initialized value.
        /// </summary>
        public void ForceInitialization()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}
