namespace OrchestratedSaga.Core.Repositories;

public class BookingTravelRepository : IBookingTravelRepository
{
    private static IList<BookingTravel> _data = new List<BookingTravel>();

    public Task AddAsync(BookingTravel entity)
    {
        _data.Add(entity);
        return Task.CompletedTask;
    }

    public Task EditAsync(BookingTravel entity)
    {
        throw new NotImplementedException();
    }

    public Task<BookingTravel?> GetByIdAsync(Guid id)
    {
        var entity = _data.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(entity);
    }
}
