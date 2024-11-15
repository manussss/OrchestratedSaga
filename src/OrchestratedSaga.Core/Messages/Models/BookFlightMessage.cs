namespace OrchestratedSaga.Core.Messages.Models;

public record BookFlightMessage(Guid BookingTravelId) : IMessage;
