using System.Linq;
using System.Threading.Tasks;
using TaskManaget.Bot.Model;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskManager.Bot.Telegram
{
    public static class TelegramResponseHandler
    {
        public static async Task SendResponse(ITelegramBotClient bot, long chatId, IResponse response)
        {
            switch (response)
            {
                case TextResponse textResponse:
                    await bot.SendTextMessageAsync(chatId, textResponse.Text);
                    break;
                case ButtonResponse buttonResponse:
                    await bot.SendTextMessageAsync(chatId, buttonResponse.Text,
                        replyMarkup: AsReplyKeyboardMarkup(buttonResponse.Buttons));
                    break;
                case InlineButtonResponse buttonResponse:
                    await bot.SendTextMessageAsync(chatId, buttonResponse.Text, replyMarkup: AsInlineButtonResponse(buttonResponse.Buttons));
                    break;
                case ChainResponse chainResponse:
                    foreach (var chainResponseResponse in chainResponse.Responses)
                        await SendResponse(bot, chatId, chainResponseResponse);
                    break;
            }
        }

        private static ReplyKeyboardMarkup AsReplyKeyboardMarkup(string[][] buttons)
        {
            return buttons;
        }

        private static InlineKeyboardMarkup AsInlineButtonResponse((string text, string callback)[][] buttons)
        {
            return buttons
                .Select(rows =>
                    rows.Select(
                            column => InlineKeyboardButton.WithCallbackData(column.text, column.callback))
                        .ToArray()
                ).ToArray();
        }
    }
}