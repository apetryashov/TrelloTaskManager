namespace Bot.Telegram.Common.Model.Session
{
    public interface ICommandSession
    {
        SessionStatus SessionStatus { get; }
        int? ContinueIndex { get; }
    }
}