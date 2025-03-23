using MassTransit;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEnpoint,IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> log) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            log.LogInformation("Domain event handled: {DomainEvent}", domainEvent.GetType().Name);
            if(await featureManager.IsEnabledAsync("OrderFullfilment"))
            {
                var eventData = domainEvent.order.ToOrderDto();
                await publishEnpoint.Publish(eventData, cancellationToken);
            }
        }
    }
}
