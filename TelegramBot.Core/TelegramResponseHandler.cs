using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Core.Model;

namespace TelegramBot.Core
{
    public static class TelegramResponseHandler
    {
        private static readonly ParseMode parseMode = ParseMode.Default;

        public static async Task SendResponse(this ITelegramBotClient bot, long chatId, IResponse response)
            => await (response switch
            {
                TextResponse r => bot.SendResponse(chatId, r),
                ButtonResponse r => bot.SendResponse(chatId, r),
                InlineButtonResponse r => bot.SendResponse(chatId, r),
                LinkResponse r => bot.SendResponse(chatId, r),
                ChainResponse r => bot.SendResponse(chatId, r)
            });

        private static async Task SendResponse(this ITelegramBotClient bot, long chatId, TextResponse response) =>
            await bot.SendTextMessageAsync(chatId, response.Text, parseMode);

        private static async Task SendResponse(this ITelegramBotClient bot, long chatId, ButtonResponse response)
        {
            ReplyKeyboardMarkup markup = response.Buttons;
            markup.ResizeKeyboard = true;

            await bot.SendTextMessageAsync(chatId, response.Text,
                parseMode,
                replyMarkup: markup);
        }

        private static async Task SendResponse(this ITelegramBotClient bot, long chatId, InlineButtonResponse response)
        {
            InlineKeyboardMarkup markup = response.Buttons
                .Select(rows =>
                    rows.Select(
                            column => InlineKeyboardButton.WithCallbackData(column.text, column.callback))
                        .ToArray()
                ).ToArray();

            await bot.SendTextMessageAsync(chatId, response.Text,
                parseMode,
                replyMarkup: markup);
        }

        private static async Task SendResponse(this ITelegramBotClient bot, long chatId, LinkResponse response)
        {
            var markup = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithUrl(response.LinkMessage, response.Link.ToString()));

            await bot.SendTextMessageAsync(chatId, response.Text,
                parseMode,
                replyMarkup: markup);
        }

        private static async Task SendResponse(this ITelegramBotClient bot, long chatId, ChainResponse response)
        {
            foreach (var chainResponseResponse in response.Responses)
                await SendResponse(bot, chatId, chainResponseResponse);
        }
    }
}