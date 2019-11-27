namespace Bot.Telegram.Common.Model.Session
{
    public interface ISession
    {
        int CommandId { get; }
        int ContinueIndex { get; }
    }
}