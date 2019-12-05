using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram.Model
{
    public class CommandResponse : ICommandResponse
    {
        public CommandResponse(IResponse response)
        {
            Response = response;
        }
        
        public CommandResponse(IResponse response, int sessionMeta)
        {
            Response = response;
            SessionMeta = new SessionMeta
            {
                ContinueFrom = sessionMeta
            };
        }

        public CommandResponse(IResponse response, ISessionMeta sessionMeta)
        {
            Response = response;
            SessionMeta = sessionMeta;
        }

        public IResponse Response { get; }
        public ISessionMeta SessionMeta { get; }
    }
}