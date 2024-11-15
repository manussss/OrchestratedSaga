var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransitInjection(builder.Configuration);
builder.Services.AddActions();

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("book-car", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<BookCarListener>();
    });

    cfg.ReceiveEndpoint("book-hotel", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<BookHotelListener>();
    });

    cfg.ReceiveEndpoint("book-flight", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<BookFlightListener>();
    });

    cfg.ReceiveEndpoint("orchestrator", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<OrchestratorListener>();
    });

    cfg.ReceiveEndpoint("compensate-book-car", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<CompensateBookCarListener>();
    });

    cfg.ReceiveEndpoint("compensate-book-hotel", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<CompensateBookHotelListener>();
    });

    cfg.ReceiveEndpoint("compensate-book-flight", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<CompensateBookFlightListener>();
    });
});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

var host = builder.Build();
host.Run();
