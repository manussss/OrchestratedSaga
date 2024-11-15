namespace OrchestratedSaga.Worker.Listeners;

public class OrchestratorListener : IConsumer<ReplyMessage>
{
    private readonly IBookingTravelRepository _bookingTravelRepository;
    private readonly  IPublishEndpoint _publishEndpoint;

    public OrchestratorListener(IServiceProvider serviceProvider)
    {
        _bookingTravelRepository = serviceProvider.GetRequiredService<IBookingTravelRepository>();
        _publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();
    }

    public async Task Consume(ConsumeContext<ReplyMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(OrchestratorListener), nameof(BookCarMessage));

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
        Serilog.Log.Information("Entity Id {id} - book car completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookCarCompleted);

        await _publishEndpoint.Publish(new BookHotelMessage(bookingTravel.Id));
    }

    private async Task HandleBookHotelCompletedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book hotel completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookHotelCompleted);

        await _publishEndpoint.Publish(new BookFlightMessage(bookingTravel.Id));
    }

    private async Task HandleBookFlightCompletedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book flight completed", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookFlightCompleted);
    }

    private async Task HandleBookCarCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book car compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookCarCompensated);
    }

    private async Task HandleBookHotelCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book hotel compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookHotelCompensated);

        await _publishEndpoint.Publish(new CancelBookCarMessage(bookingTravel.Id));

    }

    private async Task HandleBookFlightCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book flight compensated", bookingTravel.Id);

        bookingTravel.AddEvent(EventType.BookFlightCompensated);

        await _publishEndpoint.Publish(new CancelBookHotelMessage(bookingTravel.Id));
    }
}
