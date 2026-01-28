using System;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Models
{
    [Serializable]
    [Documentation("InstancedObject<T> Class", "A generic base class that ensures only one instance of a derived class exists at any time.", new string[] { "T: The type of the derived class." }, 
        @"```cs
public class MySingleton : InstancedObject<MySingleton> 
{ 
    // Your class implementation here
}
```")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Models.", "January 28, 2026")]
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