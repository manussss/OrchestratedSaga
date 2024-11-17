namespace OrchestratedSaga.Core.Extensions;

public static class SettingsSetup
{
    public static void AddSettingsInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageSettings>(configuration.GetSection("AzureStorage"));
    }
}
