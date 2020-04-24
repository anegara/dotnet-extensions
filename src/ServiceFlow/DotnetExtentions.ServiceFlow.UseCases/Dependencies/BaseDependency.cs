using System;

namespace DotnetExtentions.ServiceFlow.UseCases.Dependencies
{
    public abstract class BaseDependency
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");

        public override string ToString()
        {
            return $"{this.GetType().Name}: {_instanceKey}";
        }
    }
}
