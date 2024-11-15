namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookFlightListener : IConsumer<CancelBookFlightMessage>
{
    private readonly IAction _action;

    public CompensateBookFlightListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookFlightAction));
    }

    public async Task Consume(ConsumeContext<CancelBookFlightMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(CompensateBookFlightListener), nameof(CancelBookFlightMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
