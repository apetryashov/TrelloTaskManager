namespace Bot.Telegram.Common.Model.Session
{
    public class CommandSession : ICommandSession
    {
        public SessionStatus SessionStatus { get; set; }
        public int? ContinueIndex { get; set; }
        
        private CommandSession(){}

        public static ICommandSession SimpleCommandSession() => new CommandSession
        {
            SessionStatus = SessionStatus.Close
        };

        public static ICommandSession AbortCommandSession() => new CommandSession
        {
            SessionStatus = SessionStatus.Abort
        };

        public static ICommandSession ErrorCommandSession() => new CommandSession
        {
            SessionStatus = SessionStatus.Exception
        };

        public static ICommandSession ExpectCommandSession(int continueIndex) => new CommandSession
        {
            SessionStatus = SessionStatus.Expect,
            ContinueIndex = continueIndex
        };
    }
}