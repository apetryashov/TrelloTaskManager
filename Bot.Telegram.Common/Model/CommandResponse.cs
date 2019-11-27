using Bot.Telegram.Common.Model.Session;
using JetBrains.Annotations;

namespace Bot.Telegram.Common.Model
{
    public class CommandResponse : ICommandResponse
    {
        public CommandResponse(IResponse response)
        {
            Response = response;
            Session = CommandSession.SimpleCommandSession();
        }

        public CommandResponse(IResponse response, ICommandSession session)
        {
            Response = response;
            Session = session;
        }

        public IResponse Response { get; }
        public ICommandSession Session { get; }
    }
}