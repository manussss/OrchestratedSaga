namespace OrchestratedSaga.Core.Repositories;

public interface IBookingTravelRepository
{
    Task AddAsync(BookingTravel entity);
    Task<IEnumerable<BookingTravel>> GetAllAsync();
    Task UpdateAsync(BookingTravel entity, SagaEvent sagaEvent);
    Task<BookingTravel?> GetByIdAsync(string rowKey);
}
