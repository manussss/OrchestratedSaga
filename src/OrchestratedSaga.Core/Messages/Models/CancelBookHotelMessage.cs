namespace OrchestratedSaga.Core.Messages.Models;

public record CancelBookHotelMessage(Guid BookingTravelId) : IMessage;
