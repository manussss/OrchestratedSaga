namespace OrchestratedSaga.Core.Messages.Models;

public record CancelBookFlightMessage(string RowKey) : IMessage;
