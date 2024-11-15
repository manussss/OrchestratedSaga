namespace OrchestratedSaga.Core.Actions;

public abstract class ActionBase<T, TW> : IAction where T : IMessage where TW : IMessage
{
    protected abstract Task ExecuteAsync(T message);

    protected abstract Task CompensateAsync(TW message);

    Task IAction.ExecuteAsync<T1>(T1 message)
    {
        return ExecuteAsync((T)(IMessage)message!);
    }

    Task IAction.CompensateAsync<T1>(T1 message)
    {
        return CompensateAsync((TW)(IMessage)message!);
    }
}
