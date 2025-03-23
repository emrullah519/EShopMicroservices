using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services,
            IConfiguration appConfig,
            Assembly? assembly = null)
        {
            services.AddMassTransit(massConf =>
            {
                massConf.SetKebabCaseEndpointNameFormatter();
                if(assembly != null)
                    massConf.AddConsumers(assembly);
                massConf.UsingRabbitMq((context, rabbitConf) =>
                {
                    rabbitConf.Host(new Uri(appConfig["MessageBroker:Host"]!), (hostConf) =>
                    {
                        hostConf.Username(appConfig["MessageBroker:UserName"]!);
                        hostConf.Password(appConfig["MessageBroker:Password"]!);
                    });
                    rabbitConf.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
