using DotnetExtentions.ServiceFlow.Abstractions;
using System;
using System.Collections.Concurrent;

namespace DotnetExtentions.ServiceFlow
{
    public class ServiceFlowContext : IServiceFlowContext
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");

        private readonly ConcurrentDictionary<string, object> _container;

        public ServiceFlowContext()
        {
            _container = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);
        }

        public bool HasKey(string key)
        {
            return _container.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            return _container.TryGetValue(key, out var value) ? (T)value : default;
        }

        public void Set<T>(string key, T value)
        {
            _container.AddOrUpdate(key, value, (k, v) => value);
        }

        public bool Remove(string key)
        {
            return _container.TryRemove(key, out var value);
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
