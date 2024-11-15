namespace OrchestratedSaga.Worker.Listeners;

public class CompensateBookHotelListener : IConsumer<CancelBookHotelMessage>
{
    private readonly IAction _action;

    public CompensateBookHotelListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookHotelAction));
    }

    public async Task Consume(ConsumeContext<CancelBookHotelMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(CompensateBookHotelListener), nameof(CancelBookHotelMessage));

        var message = context.Message;

        await _action.CompensateAsync(message);
    }
}
