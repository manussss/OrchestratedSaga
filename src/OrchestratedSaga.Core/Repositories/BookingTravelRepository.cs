namespace OrchestratedSaga.Core.Repositories;

public class BookingTravelRepository : IBookingTravelRepository
{
    //TODO improvements (possible refactor, rename)
    //TODO add DI
    private const string TABLE_NAME = "BookTravel";
    private readonly TableClient _tableClient;

    public BookingTravelRepository(IOptions<StorageSettings> options)
    {
        var serviceClient = new TableServiceClient(
            new Uri($"{options.Value.Uri}{options.Value.SasToken}"),
            new TableSharedKeyCredential(options.Value.AccountName, options.Value.AccountKey));

        var tableClient = serviceClient.GetTableClient(TABLE_NAME);

        tableClient.CreateIfNotExists();

        _tableClient = new TableClient(
            new Uri(options.Value.Uri),
            TABLE_NAME,
            new TableSharedKeyCredential(options.Value.AccountName, options.Value.AccountKey));
    }

    public async Task AddAsync(BookingTravel entity)
    {
        var tableEntity = new TableEntity(entity.PartitionKey, entity.RowKey)
        {
            { "CreatedAt", entity.CreatedAt },
            { "Events", entity.EventsJson }
        };
        await _tableClient.AddEntityAsync(tableEntity);
    }

    public async Task<IEnumerable<BookingTravel>> GetAllAsync()
    {
        var entities = new List<BookingTravel>();

        var query = _tableClient.QueryAsync<BookingTravel>();

        await foreach (var entity in query)
        {
            entities.Add(entity);
        }

        return entities;
    }

    public async Task<BookingTravel?> GetByIdAsync(string rowKey)
    {
        return await _tableClient.GetEntityAsync<BookingTravel>("BookingTravel", rowKey);
    }

    public async Task UpdateAsync(BookingTravel entity, SagaEvent sagaEvent)
    {
        var sagaToUpdate = await GetByIdAsync(entity.RowKey);

        if (sagaToUpdate != null)
        {
            sagaToUpdate.LastUpdatedAt = DateTime.UtcNow;
            
            sagaToUpdate.AddEvent(sagaEvent);

            await _tableClient.UpdateEntityAsync(sagaToUpdate, sagaToUpdate.ETag);
        }
    }
}
