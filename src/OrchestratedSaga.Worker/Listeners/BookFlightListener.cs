namespace OrchestratedSaga.Worker.Listeners;

public class BookFlightListener : IConsumer<BookFlightMessage>
{
    private readonly IAction _action;

    public BookFlightListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookFlightAction));
    }

    public async Task Consume(ConsumeContext<BookFlightMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(BookFlightListener), nameof(BookFlightMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
