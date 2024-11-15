var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransitInjection(builder.Configuration);
builder.Services.AddActions();

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("book-car", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<CreateBookCarListener>();
    });

    cfg.ReceiveEndpoint("book-hotel", e =>
    {
        e.PrefetchCount = 10;
        e.UseMessageRetry(p => p.Interval(3, 100));
        e.Consumer<CreateBookHotelListener>();
    });
});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

var host = builder.Build();
host.Run();
