namespace OrchestratedSaga.Core.Actions;

public class BookHotelAction(ILogger<BookCarAction> logger, IPublishEndpoint publishEndpoint) : ActionBase<BookHotelMessage, CancelBookHotelMessage>
{
    protected override async Task ExecuteAsync(BookHotelMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.BookingTravelId, EventType.BookHotelCompleted));

        logger.LogInformation("{Event} executed", nameof(EventType.BookHotelCompleted));
    }

    protected override async Task CompensateAsync(CancelBookHotelMessage message)
    {
        await publishEndpoint.Publish(new ReplyMessage(message.BookingTravelId, EventType.BookHotelCompensated));

        logger.LogInformation("{Event} compensated", nameof(EventType.BookHotelCompensated));
    }
}
