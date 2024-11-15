namespace OrchestratedSaga.Worker.Listeners;

public class CreateBookCarListener : IConsumer<BookCarMessage>
{
    private readonly ILogger<CreateBookCarListener> _logger;
    private readonly IAction _action;

    public CreateBookCarListener(
        ILogger<CreateBookCarListener> logger, 
        [FromKeyedServices(nameof(BookCarAction))] IAction action)
    {
        _logger = logger;
        _action = action;
    }

    public CreateBookCarListener()
    {
        
    }

    public async Task Consume(ConsumeContext<BookCarMessage> context)
    {
        _logger.LogInformation("Processing {Event}", nameof(BookCarMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
