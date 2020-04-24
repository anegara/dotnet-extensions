using DotnetExtentions.ServiceFlow.Abstractions;
using DotnetExtentions.ServiceFlow.UseCases.Abstractions;
using DotnetExtentions.ServiceFlow.UseCases.Workers;
using Microsoft.Extensions.Logging;
using System;

namespace DotnetExtentions.ServiceFlow.UseCases
{
    public static class AppExtensions
    {
        public static bool HasCounter(this IServiceFlowContext ctx)
        {
            return ctx.HasKey(AppSettings.CounterKey);
        }

        public static int GetCounter(this IServiceFlowContext ctx)
        {
            //return ctx.Get<int>(AppSettings.CounterKey);

            // debug
            var x = ctx.Get<int>(AppSettings.CounterKey); ;
            return x;
        }
        
        public static void LogInformation(this ILogger<BaseWorker> logger, BaseWorker self, IServiceFlowContext ctx, ISingletonDependency sng, IScopedDependency scp, ITransientDependency trn) 
        {
            logger.LogInformation($"{self} - executing ...{Environment.NewLine} context: {ctx} | dependencies: [{sng}, {scp}, {trn}]");
        }
    }
}
