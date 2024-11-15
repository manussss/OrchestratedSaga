namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookCarListener : IConsumer<CancelBookCarMessage>
{
    private readonly IAction _action;

    public CompensateBookCarListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookCarAction));
    }

    public async Task Consume(ConsumeContext<CancelBookCarMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(CompensateBookCarListener), nameof(CancelBookCarMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
