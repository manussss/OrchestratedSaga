namespace OrchestratedSaga.Worker.Listeners;

public class BookCarListener : IConsumer<BookCarMessage>
{
    private readonly IAction _action;

    public BookCarListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookCarAction));
    }

    public async Task Consume(ConsumeContext<BookCarMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(BookCarListener), nameof(BookCarMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
