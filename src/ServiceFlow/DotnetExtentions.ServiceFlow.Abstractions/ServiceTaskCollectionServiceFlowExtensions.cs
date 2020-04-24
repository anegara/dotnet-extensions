using System;
using System.Collections.Generic;

namespace DotnetExtentions.ServiceFlow.Abstractions
{
    public static class ServiceTaskCollectionServiceFlowExtensions
    {
        public static IServiceTaskCollection AddSiblingsServiceTasks<TServiceTask, TServiceTask2>(this IServiceTaskCollection serviceTaskCollection)
            where TServiceTask : IServiceTask
            where TServiceTask2 : IServiceTask
        {
            return serviceTaskCollection.AddSiblingsServiceTasks<TServiceTask, TServiceTask2, IServiceTask>();
        }

        public static IServiceTaskCollection AddSiblingsServiceTasks<TServiceTask, TServiceTask2, TServiceTask3>(this IServiceTaskCollection serviceTaskCollection)
            where TServiceTask : IServiceTask
            where TServiceTask2 : IServiceTask
            where TServiceTask3 : IServiceTask
        {
            return serviceTaskCollection.AddSiblingsServiceTasks<TServiceTask, TServiceTask2, TServiceTask3, IServiceTask>();
        }

        public static IServiceTaskCollection AddSiblingsServiceTasks<TServiceTask, TServiceTask2, TServiceTask3, TServiceTask4>(this IServiceTaskCollection serviceTaskCollection)
            where TServiceTask : IServiceTask
            where TServiceTask2 : IServiceTask
            where TServiceTask3 : IServiceTask
            where TServiceTask4 : IServiceTask
        {
            return serviceTaskCollection.AddSiblingsServiceTasks<TServiceTask, TServiceTask2, TServiceTask3, TServiceTask4, IServiceTask>();
        }

        public static IServiceTaskCollection AddSiblingsServiceTasks<TServiceTask, TServiceTask2, TServiceTask3, TServiceTask4, TServiceTask5>(this IServiceTaskCollection serviceTaskCollection)
        where TServiceTask : IServiceTask
        where TServiceTask2 : IServiceTask
        where TServiceTask3 : IServiceTask
        where TServiceTask4 : IServiceTask
        where TServiceTask5 : IServiceTask
        {
            var instances = new List<Type>();

            if (typeof(TServiceTask).IsClass)
                instances.Add(typeof(TServiceTask));

            if (typeof(TServiceTask2).IsClass)
                instances.Add(typeof(TServiceTask2));

            if (typeof(TServiceTask3).IsClass)
                instances.Add(typeof(TServiceTask3));

            if (typeof(TServiceTask4).IsClass)
                instances.Add(typeof(TServiceTask4));

            if (typeof(TServiceTask5).IsClass)
                instances.Add(typeof(TServiceTask5));

            return serviceTaskCollection;
        }
    }
}
