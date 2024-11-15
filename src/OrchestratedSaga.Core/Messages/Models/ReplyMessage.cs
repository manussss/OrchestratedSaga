namespace OrchestratedSaga.Core.Messages.Models;

public record ReplyMessage(Guid BookingTravelId, EventType Type) : IMessage;
