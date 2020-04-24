using DotnetExtentions.ServiceFlow.Abstractions;
using System;
using System.Collections.Concurrent;

namespace DotnetExtentions.ServiceFlow
{
    public class ServiceFlowContextAsync : IServiceFlowContext
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");

        private readonly ConcurrentDictionary<string, Lazy<object>> _container;

        public ServiceFlowContextAsync()
        {
            _container = new ConcurrentDictionary<string, Lazy<object>>(StringComparer.Ordinal);
        }

        public bool HasKey(string key)
        {
            return _container.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            //return _container.TryGetValue(key, out var lazyValue) ? (T)lazyValue.Value : default;

            return GetDebug<T>(key);
        }

        public T GetDebug<T>(string key)
        {
            var x = _container.TryGetValue(key, out var lazyValue);
            if (x)
                return (T)lazyValue.Value;

            return default;
            //return _container.TryGetValue(key, out var lazyValue) ? (T)lazyValue.Value : default;
        }

        public void Set<T>(string key, T value)
        {
            var wrappedValue = new Lazy<object>();

            _container.AddOrUpdate(key, wrappedValue, (k, v) => wrappedValue);
        }

        public bool Remove(string key)
        {
            return _container.TryRemove(key, out var lazy);
        }

        public void Clear()
        {
            _container.Clear();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {_instanceKey}";
        }
    }
}
