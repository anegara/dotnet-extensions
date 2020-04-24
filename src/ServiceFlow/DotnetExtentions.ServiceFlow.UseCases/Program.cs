using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotnetExtentions.ServiceFlow.UseCases.Workers;
using DotnetExtentions.ServiceFlow.UseCases.Dependencies;
using DotnetExtentions.ServiceFlow.UseCases.Abstractions;
using DotnetExtentions.ServiceFlow.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DotnetExtentions.ServiceFlow.UseCases
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            AppSettings appSetings = hostContext.Configuration.GetSection("AppSettings").Get<AppSettings>(); ;

                            services.AddSingleton(appSetings);

                            services.AddSingleton<ISingletonDependency, SingletonDependency>();
                            services.AddScoped<IScopedDependency, ScopedDependency>();
                            services.AddTransient<ITransientDependency, TransientDependency>();

                            services.AddTransient<InitCounterWorker>();
                            services.AddTransient<IfWorker>();
                            services.AddTransient<WhileWorker>();
                            services.AddTransient<DelayWorker>();
                        })
                        .RegisterServiceFlow(flow =>
                        {
                            flow.AddServiceTask<InitCounterWorker>()
                                //.AddSiblingsServiceTasks<Worker, Worker>(); // todo
                                .AddBranchIf(ctx => ctx.HasCounter(), (IServiceTaskCollection branchFlow) =>
                                {
                                    branchFlow
                                            .AddServiceTask<IfWorker>()
                                            .AddServiceTask<IfWorker>()
                                            .AddBranchWhile(ctx => ctx.GetCounter() < 5, (IServiceTaskCollection branchFlow) =>
                                            {
                                                branchFlow.AddServiceTask<WhileWorker>();
                                            });
                                }, false)
                                .AddServiceTask<DelayWorker>();
                        });
        }
    }
}
