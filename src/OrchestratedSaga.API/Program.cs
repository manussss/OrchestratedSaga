var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMassTransitInjection(builder.Configuration);
builder.Services.AddRepositoriesInjection();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("api/v1/message", async (
    [FromBody] BookCarMessage message,
    [FromServices] IPublishEndpoint publisher,
    [FromServices] IBookingTravelRepository bookingTravelRepository,
    [FromServices] ILogger<Program> logger) =>
{
    var bookingTravel = new BookingTravel
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow
    };

    await bookingTravelRepository.AddAsync(bookingTravel);

    logger.LogInformation("Booking travel created, Id: {Id}", bookingTravel.Id);

    await publisher.Publish(new BookCarMessage(bookingTravel.Id));
    logger.LogInformation("Event sent {Event}", nameof(BookCarMessage));

    return TypedResults.Ok();
});

app.Run();
