namespace OrchestratedSaga.Core.Messages.Models;

public record BookCarMessage(Guid BookingTravelId) : IMessage;
