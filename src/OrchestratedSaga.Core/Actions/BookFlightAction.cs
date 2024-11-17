namespace OrchestratedSaga.Core.Actions;

public class BookFlightAction(ILogger<BookCarAction> logger, IPublishEndpoint publishEndpoint) : ActionBase<BookFlightMessage, CancelBookFlightMessage>
{
    protected override async Task ExecuteAsync(BookFlightMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.RowKey, EventType.BookFlightCompleted));

        logger.LogInformation("{Event} executed", nameof(EventType.BookFlightCompleted));
    }

    protected override async Task CompensateAsync(CancelBookFlightMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.RowKey, EventType.BookFlightCompensated));

        logger.LogInformation("{Event} compensated", nameof(EventType.BookFlightCompensated));
    }
}
