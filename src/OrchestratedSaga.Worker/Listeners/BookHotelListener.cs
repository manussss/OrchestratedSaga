namespace OrchestratedSaga.Worker.Listeners;

public class BookHotelListener : IConsumer<BookHotelMessage>
{
    private readonly IAction _action;

    public BookHotelListener(IServiceProvider serviceProvider)
    {
        _action = serviceProvider.GetRequiredKeyedService<IAction>(nameof(BookHotelAction));
    }

    public async Task Consume(ConsumeContext<BookHotelMessage> context)
    {
        Serilog.Log.Information("{Class} | Processing {Event}", nameof(BookHotelListener), nameof(BookHotelMessage));

        var message = context.Message;

        await _action.ExecuteAsync(message);
    }
}
