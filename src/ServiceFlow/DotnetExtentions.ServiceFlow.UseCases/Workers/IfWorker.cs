using System.Threading;
using System.Threading.Tasks;
using DotnetExtentions.ServiceFlow.Abstractions;
using DotnetExtentions.ServiceFlow.UseCases.Abstractions;
using Microsoft.Extensions.Logging;

namespace DotnetExtentions.ServiceFlow.UseCases.Workers
{
    public class IfWorker : BaseWorker, IServiceTask
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISingletonDependency _singletonDependency;
        private readonly IScopedDependency _scopedDependency;
        private readonly ITransientDependency _transientDependency;

        public IfWorker(ILogger<Worker> logger,
            ISingletonDependency singletonDependency,
            IScopedDependency scopedDependency,
            ITransientDependency transientDependency)
        {
            _logger = logger;
            _singletonDependency = singletonDependency;
            _scopedDependency = scopedDependency;
            _transientDependency = transientDependency;
        }

        public async Task ExecuteAsync(IServiceFlowContext context, CancellationToken stoppingToken)
        {
            _logger.LogInformation(this, context, _singletonDependency, _scopedDependency, _transientDependency);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
