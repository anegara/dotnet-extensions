using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetExtentions.ServiceFlow.Abstractions
{
    public interface IServiceTaskCollection
    {
        IServiceTaskCollection AddServiceTask<TServiceTask>() where TServiceTask : IServiceTask;

        IServiceTaskCollection AddServiceTask(Func<IServiceFlowContext, CancellationToken, Task> serviceTaskDelegate);

        IServiceTaskCollection AddSiblingsServiceTasks(Func<IServiceFlowContext, CancellationToken, Task>[] serviceTasks);

        IServiceTaskCollection AddBranchIf(Predicate<IServiceFlowContext> predicate, Action<IServiceTaskCollection> branchFlow, bool createNestedScope = true);

        IServiceTaskCollection AddBranchWhile(Predicate<IServiceFlowContext> predicate, Action<IServiceTaskCollection> branchFlow, bool createNestedScope = true);
    }
}
