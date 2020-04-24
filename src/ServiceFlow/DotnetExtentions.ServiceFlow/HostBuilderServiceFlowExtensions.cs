using DotnetExtentions.ServiceFlow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DotnetExtentions.ServiceFlow
{
    public static class HostBuilderServiceFlowExtensions
    {
        public static IHostBuilder RegisterServiceFlow(this IHostBuilder hostBuilder, Action<IServiceTaskCollection> configureDelegate)
        {
            var serviceFlowConfigureDelegate = configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate));

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IServiceFlowContext, ServiceFlowContext>();
                services.AddTransient(di => new HostedServiceFlow(di, configureDelegate));

                services.AddHostedService(serviceProvider =>
                {
                    return serviceProvider.GetRequiredService<HostedServiceFlow>();
                });
            });

            return hostBuilder;
        }
    }
}
