using System.Collections.Generic;
using TaskManager.Bot.Model.Session;

namespace TaskManager.Bot.Model
{
    public class ChainResponse : IResponse
    {
        private readonly List<IResponse> responses;

        private ChainResponse(SessionStatus sessionStatus)
        {
            SessionStatus = sessionStatus;
            responses = new List<IResponse>();
        }

        public IEnumerable<IResponse> Responses => responses;

        public SessionStatus SessionStatus { get; }

        public static ChainResponse Create(SessionStatus sessionStatus)
        {
            return new ChainResponse(sessionStatus);
        }

        public ChainResponse AddResponse(IResponse response)
        {
            responses.Add(response);
            return this;
        }
    }
}