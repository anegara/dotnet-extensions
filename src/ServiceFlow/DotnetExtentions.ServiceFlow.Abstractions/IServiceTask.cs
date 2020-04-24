using System.Threading;
using System.Threading.Tasks;

namespace DotnetExtentions.ServiceFlow.Abstractions
{
    public interface IServiceTask
    {
        Task ExecuteAsync(IServiceFlowContext context, CancellationToken stoppingToken);
    }
}
