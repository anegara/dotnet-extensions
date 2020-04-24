using DotnetExtentions.ServiceFlow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetExtentions.ServiceFlow
{
    public delegate Task ServiceTaskResolver(IServiceFlowContext ctx, CancellationToken cTkn);

    public class ServiceTaskCollection : IServiceTaskCollection, IDisposable
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");
        private readonly IServiceScope _serviceScope;
        private readonly ILogger<ServiceTaskCollection> _logger;

        private bool HasDisposableScope { get; set; }
        public List<ServiceTaskResolver> ServiceTaskResolvers { get; } = new List<ServiceTaskResolver>();

        private ServiceTaskCollection(IServiceScope serviceScope, Action<IServiceTaskCollection> configureDelegate, bool useInnerScope)
        {
            HasDisposableScope = useInnerScope;
            _serviceScope = useInnerScope ? serviceScope.ServiceProvider.CreateScope() : serviceScope;          
            _logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<ServiceTaskCollection>>();
            
           // _logger.LogInformation($"{this} - Creating ...");

            configureDelegate(this);

            //_logger.LogInformation($"{this} - Created.");
        }

        public ServiceTaskCollection(IServiceProvider serviceProvider, Action<IServiceTaskCollection> configureDelegate)
            : this(serviceProvider.CreateScope(), configureDelegate, false)
        {
            this.HasDisposableScope = true;
        }

        public static ServiceTaskCollection CreateRoot(IServiceProvider serviceProvider, Action<IServiceTaskCollection> configureDelegate)
        {
            return  new ServiceTaskCollection(serviceProvider, configureDelegate);
        }

        public void Dispose() 
        {
            //_logger.LogInformation($"{this} - Disposing ...");

            ServiceTaskResolvers.Clear();
            
            if (HasDisposableScope)
                _serviceScope.Dispose();

            //_logger.LogInformation($"{this} - Disposed.");
        }

        #region IServiceFlow

        public IServiceTaskCollection AddServiceTask(Func<IServiceFlowContext, CancellationToken, Task> serviceTask)
        {
            //_logger.LogInformation($"{this} - AddServiceTask.");

            ServiceTaskResolvers.Add((ctx, cTkn) => { return serviceTask(ctx, cTkn); });
            
            return this;
        }

        public IServiceTaskCollection AddServiceTask<TServiceTask>() where TServiceTask : IServiceTask
        {
            //_logger.LogInformation($"{this} - AddServiceTask<>.");

            Func<IServiceFlowContext, CancellationToken, Task> resolver = (ctx, stoppingToken) =>
            {
                var serticeTask = _serviceScope.ServiceProvider.GetRequiredService<TServiceTask>();
                return serticeTask.ExecuteAsync(ctx, stoppingToken);
            };

            return AddServiceTask(resolver);
        }

        public IServiceTaskCollection AddBranchIf(Predicate<IServiceFlowContext> predicate, Action<IServiceTaskCollection> branchFlow, bool useInnerScope = true)
        {
            //_logger.LogInformation($"{this} - AddBranchIf.");

            Func<IServiceFlowContext, CancellationToken, Task> resolver = (ctx, stoppingToken) =>
            {
                if (predicate(ctx) == false)
                    return Task.CompletedTask;

                using (var serviceTaskCollection = new ServiceTaskCollection(_serviceScope, branchFlow, useInnerScope))
                {
                    //_logger.LogInformation($"{this} - executing collection: {serviceTaskCollection} ...");
                    ServiceTaskCollectionEngine.Run(serviceTaskCollection, ctx, stoppingToken);
                    //_logger.LogInformation($"{this} - executed collection: {serviceTaskCollection}.");

                    return Task.CompletedTask;
                }
            };

            return AddServiceTask(resolver);
        }

        public IServiceTaskCollection AddBranchWhile(Predicate<IServiceFlowContext> predicate, Action<IServiceTaskCollection> branchFlow, bool useInnerScope = true)
        {
            //_logger.LogInformation($"{this} - AddBranchWhile.");

            Func<IServiceFlowContext, CancellationToken, Task> resolver = (ctx, stoppingToken) =>
            {
                while (predicate(ctx))
                {
                    using (var serviceTaskCollection = new ServiceTaskCollection(_serviceScope, branchFlow, useInnerScope))
                    {
                        //_logger.LogInformation($"{this} - executing collection: {serviceTaskCollection} ...");
                        ServiceTaskCollectionEngine.Run(serviceTaskCollection, ctx, stoppingToken);
                        //_logger.LogInformation($"{this} - executed collection: {serviceTaskCollection}.");
                    }
                }

                return Task.CompletedTask;
            };

            return AddServiceTask(resolver);
        }

        public IServiceTaskCollection AddSiblingsServiceTasks(Func<IServiceFlowContext, CancellationToken, Task>[] serviceTasks)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override string ToString()
        {
            return $"{this.GetType().Name}: {_instanceKey} [{ServiceTaskResolvers.Count}]";
        }
    }
}
