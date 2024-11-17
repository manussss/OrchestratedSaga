namespace OrchestratedSaga.Core.Messages.Models;

public record ReplyMessage(string RowKey, EventType Type) : IMessage;
