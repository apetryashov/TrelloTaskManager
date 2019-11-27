namespace Bot.Telegram.Common.Model.Session
{
    public class Session : ISession
    {
        public Session(int commandId, int continueIndex)
        {
            CommandId = commandId;
            ContinueIndex = continueIndex;
        }

        public int CommandId { get; }
        public int ContinueIndex { get; }
    }
}