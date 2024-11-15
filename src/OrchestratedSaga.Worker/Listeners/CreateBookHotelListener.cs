namespace OrchestratedSaga.Worker.Listeners;

public class CreateBookHotelListener : IConsumer<BookHotelMessage>
{
    private readonly ILogger<CreateBookHotelListener> _logger;
    private readonly IAction _action;

    public CreateBookHotelListener(
        ILogger<CreateBookHotelListener> logger,
        [FromKeyedServices(nameof(BookHotelAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public CreateBookHotelListener()
    {
        
    }

    public async Task Consume(ConsumeContext<BookHotelMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookHotelMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
