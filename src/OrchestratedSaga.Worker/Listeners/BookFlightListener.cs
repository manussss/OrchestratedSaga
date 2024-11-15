namespace OrchestratedSaga.Worker.Listeners;

public class BookFlightListener : IConsumer<BookFlightMessage>
{
    private readonly ILogger<BookFlightListener> _logger;
    private readonly IAction _action;

    public BookFlightListener(
        ILogger<BookFlightListener> logger,
        [FromKeyedServices(nameof(BookFlightAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public BookFlightListener()
    {

    }

    public async Task Consume(ConsumeContext<BookFlightMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookFlightMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
