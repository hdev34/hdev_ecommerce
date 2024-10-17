using Newtonsoft.Json;
namespace EventBus.Base.Events;

public class IntegrationEvent
{
    [JsonProperty]
    public Guid Id { get; private set; }
    [JsonProperty]
    public DateTime CratedDate { get; private set; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CratedDate = DateTime.Now;
    }
    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime cratedDate)
    {
        Id = id;
        CratedDate = cratedDate;
    }
}
