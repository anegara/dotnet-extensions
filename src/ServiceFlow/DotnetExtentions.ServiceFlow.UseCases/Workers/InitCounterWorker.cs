using System.Threading;
using System.Threading.Tasks;
using DotnetExtentions.ServiceFlow.Abstractions;
using DotnetExtentions.ServiceFlow.UseCases.Abstractions;
using Microsoft.Extensions.Logging;

namespace DotnetExtentions.ServiceFlow.UseCases.Workers
{
    public class InitCounterWorker : BaseWorker, IServiceTask
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<Worker> _logger;
        private readonly ISingletonDependency _singletonDependency;
        private readonly IScopedDependency _scopedDependency;
        private readonly ITransientDependency _transientDependency;

        public InitCounterWorker(AppSettings appSettings,
            ILogger<Worker> logger,
            ISingletonDependency singletonDependency,
            IScopedDependency scopedDependency,
            ITransientDependency transientDependency)
        {
            _appSettings = appSettings;
            _logger = logger;
            _singletonDependency = singletonDependency;
            _scopedDependency = scopedDependency;
            _transientDependency = transientDependency;
        }

        public async Task ExecuteAsync(IServiceFlowContext context, CancellationToken stoppingToken)
        {
            _logger.LogInformation(this, context, _singletonDependency, _scopedDependency, _transientDependency);

            context.Set(AppSettings.CounterKey, _appSettings.CounterStartsAt);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
