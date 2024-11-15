namespace OrchestratedSaga.Worker.Listeners;

public class BookCarListener : IConsumer<BookCarMessage>
{
    private readonly ILogger<BookCarListener> _logger;
    private readonly IAction _action;

    public BookCarListener(
        ILogger<BookCarListener> logger, 
        [FromKeyedServices(nameof(BookCarAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public BookCarListener()
    {
        
    }

    public async Task Consume(ConsumeContext<BookCarMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookCarMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
