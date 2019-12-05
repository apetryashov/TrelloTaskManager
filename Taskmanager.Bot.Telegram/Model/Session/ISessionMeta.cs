namespace TaskManager.Bot.Telegram.Model.Session
{
    public interface ISessionMeta
    {
        int ContinueFrom { get; }
    }
}