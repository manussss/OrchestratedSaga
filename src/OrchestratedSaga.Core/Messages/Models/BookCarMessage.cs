namespace OrchestratedSaga.Core.Messages.Models;

public record BookCarMessage(string RowKey) : IMessage;
