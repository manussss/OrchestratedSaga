namespace OrchestratedSaga.Core.Messages.Models;

public record CancelBookCarMessage(Guid BookingTravelId) : IMessage;
