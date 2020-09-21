using System.Collections.Generic;

namespace TelegramBot.Core.Model
{
    public class ChainResponse : IResponse
    {
        private readonly List<IResponse> responses;

        private ChainResponse() => responses = new List<IResponse>();

        public IEnumerable<IResponse> Responses => responses;

        public static ChainResponse Create() => new ChainResponse();

        public ChainResponse AddResponse(IResponse response)
        {
            responses.Add(response);
            return this;
        }
    }
}