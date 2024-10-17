using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Events;

public abstract class BaseEventBus : IEventBus
{
    public readonly IServiceProvider serviceProvider;
    public readonly IEventBusSubscriptionManager subsManager;
    private EventBusConfig eventBusConfig;

    public BaseEventBus(IServiceProvider serviceProvider, EventBusConfig eventBusConfig)
    {
        this.serviceProvider = serviceProvider;
        this.eventBusConfig = eventBusConfig;
        subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    public virtual string ProcessEventName(string eventName)
    {
        if (eventBusConfig.DeleteEventPrefix) eventName = eventName.TrimStart(eventBusConfig.EventNamePrefix.ToArray());
        if (eventBusConfig.DeleteEventSuffix) eventName = eventName.TrimStart(eventBusConfig.EventNameSuffix.ToArray());
        return eventName;
    }
    public virtual string GetSubName(string eventName) => $"{eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";

    public virtual void Dispose() => eventBusConfig = null;

    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);
        var processed = false;

        if (subsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = subsManager.GetHandlersForEvent(eventName);
            using (var scope = serviceProvider.CreateScope())
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = serviceProvider.GetService(subscription.HandlerType);
                    if (handler is null) continue;
                    var eventType = subsManager.GetEventTypeByName($"{eventBusConfig.EventNamePrefix}{eventName}{eventBusConfig.EventNameSuffix}");
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
            processed = true;
        }
        return processed;
    }


    public void Publish(IntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public void UnSubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }
}
