namespace TaskManager.Bot.Model.Session
{
    public interface ISession
    {
        int CommandId { get; }
        ISessionMeta SessionMeta { get; }
    }
}