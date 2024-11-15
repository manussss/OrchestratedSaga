namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookCarListener : IConsumer<CancelBookCarMessage>
{
    private readonly ILogger<CompensateBookCarListener> _logger;
    private readonly IAction _action;

    public CompensateBookCarListener(
        ILogger<CompensateBookCarListener> logger,
        [FromKeyedServices(nameof(BookCarAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public CompensateBookCarListener()
    {

    }

    public async Task Consume(ConsumeContext<CancelBookCarMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(CancelBookCarMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
