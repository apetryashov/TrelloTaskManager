using System.Threading.Tasks;
using TaskManager.Bot.Telegram.Model;
using Telegram.Bot;

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
                    await bot.SendTextMessageAsync(chatId, buttonResponse.Text, replyMarkup: buttonResponse.Buttons);
                    break;
                case InlineButtonResponse buttonResponse:
                    await bot.SendTextMessageAsync(chatId, buttonResponse.Text, replyMarkup: buttonResponse.Buttons);
                    break;
                case ChainResponse chainResponse:
                    foreach (var chainResponseResponse in chainResponse.Responses)
                        await SendResponse(bot, chatId, chainResponseResponse);
                    break;
            }
        }
    }
}