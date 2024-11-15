namespace OrchestratedSaga.Core.Entities;

public class BookingTravel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public IList<string> Events { get; set; } = new List<string>();

    public void AddEvent(EventType @event)
    {
        Events.Add(@event.ToString());
    }
}
