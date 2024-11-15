namespace OrchestratedSaga.Worker.Listeners;

public class BookHotelListener : IConsumer<BookHotelMessage>
{
    private readonly ILogger<BookHotelListener> _logger;
    private readonly IAction _action;

    public BookHotelListener(
        ILogger<BookHotelListener> logger,
        [FromKeyedServices(nameof(BookHotelAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public BookHotelListener()
    {
        
    }

    public async Task Consume(ConsumeContext<BookHotelMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookHotelMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
