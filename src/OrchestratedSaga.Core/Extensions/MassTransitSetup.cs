namespace OrchestratedSaga.Core.Extensions;

public static class MassTransitSetup
{
    public static void AddMassTransitInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(bus =>
        {
            bus.UsingRabbitMq((ctx, busConfigurator) =>
            {
                busConfigurator.Host(configuration.GetConnectionString("RabbitMq"));
            });
        });
    }
}
