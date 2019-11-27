using Bot.Telegram.Common.Model;

namespace Bot.Telegram.Common
{
    public static class TelegramResponseHandler
    {
        public static BotData ResponseAnalyzer(IResponse response)
        {
            return response switch
            {
                ButtonResponse buttonAnswer => new BotData(buttonAnswer.Text, buttonAnswer.Buttons),
                TextResponse textAnswer => new BotData(textAnswer.Text),
                _ => new BotData(response.Text)
            };
        }
    }
}