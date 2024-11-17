namespace OrchestratedSaga.Core.Entities;

public class BookingTravel : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    // Estado atual da saga
    public string CurrentState { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string EventsJson { get; set; }

    private List<SagaEvent>? _events;

    [IgnoreDataMember]
    public List<SagaEvent> Events
    {
        get
        {
            // Se a lista ainda não foi carregada, desserialize de EventsJson
            if (_events == null)
            {
                _events = string.IsNullOrEmpty(EventsJson)
                    ? new List<SagaEvent>()
                    : JsonSerializer.Deserialize<List<SagaEvent>>(EventsJson) ?? new List<SagaEvent>();
            }
            return _events;
        }
        set
        {
            _events = value;
            EventsJson = JsonSerializer.Serialize(_events);
        }
    }

    public void AddEvent(SagaEvent e)
    {
        Events.Add(e);
        EventsJson = JsonSerializer.Serialize(Events);
    }

    public BookingTravel()
    {
        CreatedAt = DateTime.UtcNow;
        PartitionKey = "BookingTravel";
        RowKey = Guid.NewGuid().ToString();
        Events = [];
        EventsJson = "[]";
    }
}

public class SagaEvent
{
    public string EventId { get; set; }
    public string EventType { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Payload { get; set; }
}
