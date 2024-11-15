namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookFlightListener : IConsumer<CancelBookFlightMessage>
{
    private readonly ILogger<CompensateBookFlightListener> _logger;
    private readonly IAction _action;

    public CompensateBookFlightListener(
        ILogger<CompensateBookFlightListener> logger,
        [FromKeyedServices(nameof(BookFlightAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public CompensateBookFlightListener()
    {

    }

    public async Task Consume(ConsumeContext<CancelBookFlightMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(CancelBookFlightMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
