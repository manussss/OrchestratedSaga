namespace OrchestratedSaga.Core.Extensions;

public static class ActionsSetup
{
    public static void AddActions(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IAction, BookFlightAction>(nameof(BookFlightAction));
        services.AddKeyedSingleton<IAction, BookHotelAction>(nameof(BookHotelAction));
        services.AddKeyedSingleton<IAction, BookCarAction>(nameof(BookCarAction));
    }
}
