namespace OrchestratedSaga.Core.Repositories;

public interface IBookingTravelRepository
{
    Task AddAsync(BookingTravel entity);
    Task EditAsync(BookingTravel entity);
    Task<BookingTravel?> GetByIdAsync(Guid id);
}
