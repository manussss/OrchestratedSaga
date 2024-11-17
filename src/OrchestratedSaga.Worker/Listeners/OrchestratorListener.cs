namespace OrchestratedSaga.Worker.Listeners;

public class OrchestratorListener : IConsumer<ReplyMessage>
{
    private readonly IBookingTravelRepository _bookingTravelRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrchestratorListener(IServiceProvider serviceProvider)
    {
        _bookingTravelRepository = serviceProvider.GetRequiredService<IBookingTravelRepository>();
        _publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();
    }

    public async Task Consume(ConsumeContext<ReplyMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(OrchestratorListener), context.Message.Type.ToString());

        var message = context.Message;

        var bookingTravel = await _bookingTravelRepository.GetByIdAsync(message!.RowKey);

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
        Serilog.Log.Information("Entity Id {id} - book car completed", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookCarCompleted.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });

        await _publishEndpoint.Publish(new BookHotelMessage(bookingTravel.RowKey));
    }

    private async Task HandleBookHotelCompletedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book hotel completed", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookHotelCompleted.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });

        await _publishEndpoint.Publish(new BookFlightMessage(bookingTravel.RowKey));
    }

    private async Task HandleBookFlightCompletedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book flight completed", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookFlightCompleted.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });
    }

    private async Task HandleBookCarCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book car compensated", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookCarCompensated.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });
    }

    private async Task HandleBookHotelCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book hotel compensated", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookHotelCompensated.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });

        await _publishEndpoint.Publish(new CancelBookCarMessage(bookingTravel.RowKey));
    }

    private async Task HandleBookFlightCompensatedAsync(BookingTravel bookingTravel)
    {
        Serilog.Log.Information("Entity Id {id} - book flight compensated", bookingTravel.RowKey);

        await _bookingTravelRepository.UpdateAsync(bookingTravel, new SagaEvent
        {
            EventId = Guid.NewGuid().ToString(),
            EventType = EventType.BookFlightCompensated.ToString(),
            Timestamp = DateTime.UtcNow,
            Payload = new Dictionary<string, object>
            {
                { "BookingTravel", bookingTravel.RowKey }
            }
        });

        await _publishEndpoint.Publish(new CancelBookHotelMessage(bookingTravel.RowKey));
    }
}
