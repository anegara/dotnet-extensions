namespace DotnetExtentions.ServiceFlow.Abstractions
{
    public interface IServiceFlowContext
    {
        bool HasKey(string key);
        T Get<T>(string key);
        void Set<T>(string key, T value);
        bool Remove(string key);

        void Clear();
    }
}
