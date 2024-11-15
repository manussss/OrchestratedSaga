namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookHotelListener : IConsumer<CancelBookHotelMessage>
{
    private readonly ILogger<CompensateBookHotelListener> _logger;
    private readonly IAction _action;

    public CompensateBookHotelListener(
        ILogger<CompensateBookHotelListener> logger,
        [FromKeyedServices(nameof(BookHotelAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public CompensateBookHotelListener()
    {

    }

    public async Task Consume(ConsumeContext<CancelBookHotelMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(CancelBookHotelMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
