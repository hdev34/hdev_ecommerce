namespace EventBus.Base;

public class EventBusConfig
{
    public int ConnectionRetryCount { get; set; } = 5; //Bağlanmaya çalışma sayısı. 
    public string DefaultTopicName { get; set; } = "hdevEventBus"; 
    public string EventBusConnectionString { get; set; } = String.Empty;
    public string SubscriberClientAppName { get; set; } = String.Empty; //Bir eventi birkaç servis dinleyebilceği için hangi servisin olayı işlediğini belirtmek için.
    public string EventNamePrefix { get; set; } = String.Empty;
    public string EventNameSuffix { get; set; } = "IntegrationEvent";
    public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;
    public object Connection { get; set; } //Rabbit dışında QP kullanma durumu için.

    public bool DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
    public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);

}
public enum EventBusType
{
    RabbitMQ = 0,
    AzureServiceBus = 1
}
