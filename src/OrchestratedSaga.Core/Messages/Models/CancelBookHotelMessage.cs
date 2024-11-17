namespace OrchestratedSaga.Core.Messages.Models;

public record CancelBookHotelMessage(string RowKey) : IMessage;
