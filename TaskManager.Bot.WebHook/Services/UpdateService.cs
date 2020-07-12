using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManager.Bot.Model;
using TaskManager.Bot.Model.Domain;
using TaskManager.Bot.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskManager.Bot.WebHook.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly TelegramBotClient client;
        private readonly ILogger<UpdateService> logger;
        private readonly IRequestHandler requestHandler;

        public UpdateService(ILogger<UpdateService> logger, IRequestHandler requestHandler, TelegramBotClient client)
        {
            this.logger = logger;
            this.requestHandler = requestHandler;
            this.client = client;
        }

        public async Task EchoAsync(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    await BotOnCallbackQueryReceived(update.CallbackQuery);
                    return;
                case UpdateType.Message:
                    await BotOnMessageReceived(update.Message);
                    break;
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            try
            {
                var request = AnalyzeIncomingMessage(message);
                await HandleIncomingRequest(request);
            }
            catch (ArgumentException e)
            {
                await client.SendTextMessageAsync(message.Chat.Id, e.Message);
            }
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery query)
        {
            try
            {
                var request = AnalyzeIncomingCallback(query);
                await HandleIncomingRequest(request);
            }
            catch (ArgumentException e)
            {
                await client.SendTextMessageAsync(query.Message.Chat.Id, e.Message);
            }
        }

        private async Task HandleIncomingRequest(IRequest request)
        {
            var response = requestHandler.GetResponse(request);
            await TelegramResponseHandler.SendResponse(client, request.Author.TelegramId, response);
        }

        private static IRequest AnalyzeIncomingMessage(Message message)
        {
            if (message.Type != MessageType.Text)
                throw new ArgumentException($"{message.Type} is not supported yet =[");
            var sender = message.Chat;
            var author = new Author(sender.Id, sender.FirstName, sender.LastName, sender.Username);
            return new BaseRequest(author, message.Text, message.Chat.Id);
        }

        private static IRequest AnalyzeIncomingCallback(CallbackQuery callback)
        {
            var sender = callback.Message.Chat;
            var author = new Author(sender.Id, sender.FirstName, sender.LastName, sender.Username);
            return new BaseRequest(author, callback.Data, sender.Id);
        }
    }
}