using DotnetExtentions.ServiceFlow.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetExtentions.ServiceFlow
{
    public class ServiceTaskCollectionEngine
    {
        public static async Task RunAsync(ServiceTaskCollection serviceTaskCollection, IServiceFlowContext context, CancellationToken token)
        {
            var x = serviceTaskCollection.ServiceTaskResolvers.ToList();
            foreach (var serviceTask in x)
            {
                await serviceTask(context, token).ConfigureAwait(false);
            }
        }

        public static void Run(ServiceTaskCollection serviceTaskCollection, IServiceFlowContext context, CancellationToken token)
        {
            var x = serviceTaskCollection.ServiceTaskResolvers.ToList();
            foreach (var serviceTask in x)
            {
                serviceTask(context, token).Wait();
            }
        }
    }
}
