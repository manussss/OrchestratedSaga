namespace OrchestratedSaga.Core.Extensions;

public static class SerilogSetup
{
    public static void AddSerilog(this IServiceCollection services, string applicationName)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("MassTransit", LogEventLevel.Error)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", $"{applicationName}")
            .WriteTo.Async(writeTo => writeTo.Console(LogEventLevel.Information))
            .CreateLogger();
    }
}
