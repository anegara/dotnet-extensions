using DotnetExtentions.ServiceFlow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetExtentions.ServiceFlow
{
    public class HostedServiceFlow : BackgroundService
    {
        private readonly string _instanceKey = Guid.NewGuid().ToString("N");
        private ulong _cycleCounter = 0;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HostedServiceFlow> _logger;
        private readonly IServiceFlowContext _serviceFlowContext;
        private readonly Action<IServiceTaskCollection> _configureDelegate;

        public HostedServiceFlow(IServiceProvider serviceProvider, Action<IServiceTaskCollection> configureDelegate)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<HostedServiceFlow>>();
            _serviceFlowContext = _serviceProvider.GetRequiredService<IServiceFlowContext>();
            _configureDelegate = configureDelegate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{Environment.NewLine}{Environment.NewLine}{this} - running service flow cycle: {++_cycleCounter}");

                _serviceFlowContext.Clear();

                using (var serviceTaskCollection = ServiceTaskCollection.CreateRoot(_serviceProvider, _configureDelegate))
                {
                    //_logger.LogInformation($"{this} - executing collection: {serviceTaskCollection} ...");
                    ServiceTaskCollectionEngine.Run(serviceTaskCollection, _serviceFlowContext, stoppingToken);
                   // _logger.LogInformation($"{this} - executed collection: {serviceTaskCollection}.");
                }
            }

            await Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {_instanceKey}";
        }
    }
}
