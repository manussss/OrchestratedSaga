namespace OrchestratedSaga.Core.Messages.Models;

public record CancelBookCarMessage(string RowKey) : IMessage;
