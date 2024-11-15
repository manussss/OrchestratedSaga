namespace OrchestratedSaga.Core.Extensions;

public static class RepositoriesSetup
{
    public static void AddRepositoriesInjection(this IServiceCollection services)
    {
        services.AddTransient<IBookingTravelRepository, BookingTravelRepository>();
    }
}
