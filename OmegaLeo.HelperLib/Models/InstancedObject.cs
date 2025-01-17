using System;

namespace GameDevLibrary.Models
{
    public abstract class InstancedObject<T> where T : class
    {
        public static T Instance { get; private set; }

        protected InstancedObject()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException($"An instance of {typeof(T).Name} already exists.");
            }

            Instance = this as T;
            if (Instance == null)
            {
                throw new InvalidOperationException($"Failed to cast {GetType().Name} to {typeof(T).Name}.");
            }
        }

        ~InstancedObject()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}