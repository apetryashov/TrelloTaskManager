namespace Bot.Telegram.Common.Model.Session
{
    public class Session : ISession
    {
        public Session(int commandId, ISessionMeta sessionMeta)
        {
            CommandId = commandId;
            SessionMeta = sessionMeta;
        }

        public int CommandId { get; }
        public ISessionMeta SessionMeta { get; }
    }
}