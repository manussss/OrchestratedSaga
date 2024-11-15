namespace OrchestratedSaga.Core.Actions;

public class BookCarAction(ILogger<BookCarAction> logger, IPublishEndpoint publishEndpoint) : ActionBase<BookCarMessage, CancelBookCarMessage>
{
    protected override async Task ExecuteAsync(BookCarMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.BookingTravelId, EventType.BookCarCompleted));

        logger.LogInformation("{Event} executed", nameof(EventType.BookCarCompleted));
    }

    protected override async Task CompensateAsync(CancelBookCarMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.BookingTravelId, EventType.BookCarCompensated));

        logger.LogInformation("{Event} compensated", nameof(EventType.BookCarCompensated));
    }
}
