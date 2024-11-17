namespace OrchestratedSaga.Core.Messages.Models;

public record BookFlightMessage(string RowKey) : IMessage;
