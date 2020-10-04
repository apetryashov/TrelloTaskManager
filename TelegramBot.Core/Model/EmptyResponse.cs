namespace TelegramBot.Core.Model
{
    public class EmptyResponse : IResponse
    {
        private EmptyResponse(){}
        public static IResponse Create() => new EmptyResponse();
    }
}