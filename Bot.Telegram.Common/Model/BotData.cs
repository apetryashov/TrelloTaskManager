using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Telegram.Common.Model
{
    public class BotData
    {
        public BotData(string text, IReplyMarkup markup = null)
        {
            Text = text;
            Markup = markup;
        }

        public string Text { get; }
        public IReplyMarkup Markup { get; }
    }
}