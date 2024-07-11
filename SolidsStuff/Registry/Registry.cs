using System;
using System.Collections.Generic;

namespace SolidLib.Registry
{
    public class Registry<T>
    {
        private readonly Dictionary<string, T> _registry = new Dictionary<string, T>();

        public void Register(string name, T key)
        {
            if (_registry.ContainsKey(name))
            {
                SolidLib.Log.LogWarning($"[Registry] Registry with name {name} is already registered.");
            }
            _registry[name] = key;
        }

        public bool Contains(string name)
        {
            if (_registry.ContainsKey(name))
            {
                return true;
            }

            return false;
        }

        public T Get(string name)
        {
            if (_registry.TryGetValue(name, out var key))
            {
                return key;
            }
            SolidLib.Log.LogWarning($"[Registry] Registry with name {name} not found.");
            
            if (default(T) == null) 
            {
                return default(T);
            }

            return default;
        }

        public bool TryGetValue(string name, out T item)
        {
            return _registry.TryGetValue(name, out item);
        }

        public IEnumerable<T> GetAll()
        {
            return _registry.Values;
        }
    }
}
