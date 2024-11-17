var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMassTransitInjection(builder.Configuration);
builder.Services.AddRepositoriesInjection();
builder.Services.AddLogging();
builder.Services.AddSettingsInjection(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("api/v1/message", async (
    [FromServices] IPublishEndpoint publisher,
    [FromServices] IBookingTravelRepository bookingTravelRepository,
    [FromServices] ILogger<Program> logger) =>
{
    var bookingTravel = new BookingTravel();

    await bookingTravelRepository.AddAsync(bookingTravel);

    logger.LogInformation("Booking travel created, Id: {Id}", bookingTravel.RowKey);

    await publisher.Publish(new BookCarMessage(bookingTravel.RowKey));
    logger.LogInformation("Event sent {Event}", nameof(BookCarMessage));

    return TypedResults.Ok();
});

app.MapGet("api/v1/message/{rowKey}", async (
    [FromServices] IBookingTravelRepository bookingTravelRepository,
    string rowKey) =>
{
    var bookingTravel = await bookingTravelRepository.GetByIdAsync(rowKey);

    if (bookingTravel == null)
        return Results.NotFound();

    return Results.Ok(bookingTravel);
});

app.MapGet("api/v1/message", async (
    [FromServices] IBookingTravelRepository bookingTravelRepository) =>
{
    var bookingTravels = await bookingTravelRepository.GetAllAsync();

    if (bookingTravels == null)
        return Results.NotFound();

    return Results.Ok(bookingTravels);
});

app.Run();
