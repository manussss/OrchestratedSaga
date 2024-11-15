namespace OrchestratedSaga.Worker.Listeners;

public class OrchestratorListener : IConsumer<ReplyMessage>
{
    private readonly ILogger<BookFlightListener> _logger;
    private readonly IBookingTravelRepository _bookingTravelRepository;
    private readonly  IPublishEndpoint _publishEndpoint;

    public OrchestratorListener(
        ILogger<BookFlightListener> logger,
        IBookingTravelRepository bookingTravelRepository,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _bookingTravelRepository = bookingTravelRepository;
        _publishEndpoint = publishEndpoint;
    }

    public OrchestratorListener()
    {
        
    }

    public async Task Consume(ConsumeContext<ReplyMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookCarMessage));

        var message = context.Message;

        var bookingTravel = await _bookingTravelRepository.GetByIdAsync(message!.BookingTravelId);

        if (bookingTravel is null)
            return;

        switch (message.Type)
        {
            case EventType.BookCarCompleted:
                await HandleBookCarCompletedAsync(bookingTravel);
                break;

            case EventType.BookHotelCompleted:
                await HandleBookHotelCompletedAsync(bookingTravel);
                break;

            case EventType.BookFlightCompleted:
                await HandleBookFlightCompletedAsync(bookingTravel);
                break;

            case EventType.BookCarCompensated:
                await HandleBookCarCompensatedAsync(bookingTravel);
                break;

            case EventType.BookHotelCompensated:
                await HandleBookHotelCompensatedAsync(bookingTravel);
                break;

            case EventType.BookFlightCompensated:
                await HandleBookFlightCompensatedAsync(bookingTravel);
                break;
        }
    }

    private async Task HandleBookCarCompletedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book car completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookCarCompleted);

        await _publishEndpoint.Publish(new BookHotelMessage(bookingTravel.Id));
    }

    private async Task HandleBookHotelCompletedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book hotel completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookHotelCompleted);

        await _publishEndpoint.Publish(new BookFlightMessage(bookingTravel.Id));
    }

    private async Task HandleBookFlightCompletedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book flight completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookFlightCompleted);
    }

    private async Task HandleBookCarCompensatedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book car compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookCarCompensated);
    }

    private async Task HandleBookHotelCompensatedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book hotel compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookHotelCompensated);

        await _publishEndpoint.Publish(new CancelBookCarMessage(bookingTravel.Id));

    }

    private async Task HandleBookFlightCompensatedAsync(BookingTravel bookingTravel)
    {
        _logger.LogInformation("Entity Id {id} - book flight compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookFlightCompensated);

        await _publishEndpoint.Publish(new CancelBookHotelMessage(bookingTravel.Id));
    }
}
