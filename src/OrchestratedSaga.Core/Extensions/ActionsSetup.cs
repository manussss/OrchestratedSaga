namespace OrchestratedSaga.Core.Extensions;

public static class ActionsSetup
{
    public static void AddActions(this IServiceCollection services)
    {
        services.AddKeyedScoped<IAction, BookFlightAction>(nameof(BookFlightAction));
        services.AddKeyedScoped<IAction, BookHotelAction>(nameof(BookHotelAction));
        services.AddKeyedScoped<IAction, BookCarAction>(nameof(BookCarAction));
    }
}
