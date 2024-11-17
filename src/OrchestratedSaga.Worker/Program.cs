var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransitInjection(builder.Configuration);
builder.Services.AddRepositoriesInjection();
builder.Services.AddActions();
builder.Services.AddSerilog("Orchestrator");
builder.Services.AddScoped<BookCarListener>();
builder.Services.AddScoped<BookFlightListener>();
builder.Services.AddScoped<BookHotelListener>();
builder.Services.AddScoped<OrchestratorListener>();
builder.Services.AddScoped<CompensateBookCarListener>();
builder.Services.AddScoped<CompensateBookFlightListener>();
builder.Services.AddScoped<CompensateBookHotelListener>();
builder.Services.AddSettingsInjection(builder.Configuration);

var host = builder.Build();

using var scope = host.Services.CreateScope();
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
cfg.ReceiveEndpoint("book-car", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<BookCarListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("book-hotel", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<BookHotelListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("book-flight", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<BookFlightListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("orchestrator", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<OrchestratorListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("compensate-book-car", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<CompensateBookCarListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("compensate-book-hotel", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<CompensateBookHotelListener>(scope.ServiceProvider);
});

cfg.ReceiveEndpoint("compensate-book-flight", e =>
{
    e.PrefetchCount = 10;
    e.UseMessageRetry(p => p.Interval(3, 100));
    e.Consumer<CompensateBookFlightListener>(scope.ServiceProvider);
});
});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

host.Run();
