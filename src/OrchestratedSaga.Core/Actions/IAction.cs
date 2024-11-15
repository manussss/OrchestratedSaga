namespace OrchestratedSaga.Core.Actions;

public interface IAction
{
    public Task ExecuteAsync<T>(T message);

    public Task CompensateAsync<T>(T message);
}
