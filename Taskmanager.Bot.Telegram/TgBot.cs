using System;
using System.Threading.Tasks;
using TaskManaget.Bot;
using TaskManaget.Bot.Model;
using TaskManaget.Bot.Model.Domain;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskManager.Bot.Telegram
{
    public class TgBot : IBot
    {
        private readonly ITelegramBotClient bot;
        private readonly IRequestHandler requestHandler;

        public TgBot(ITelegramBotClient bot, IRequestHandler requestHandler)
        {
            this.bot = bot;
            this.requestHandler = requestHandler;
        }

        public event Action<IRequest> OnRequest;
        public event Action<Exception> OnError;

        public void Start()
        {
            bot.StartReceiving();
            bot.OnMessage += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQueryReceived;
        }

        public void Stop()
        {
            bot.StopReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            try
            {
                var request = AnalyzeIncomingMessage(message);
                await HandleIncomingRequest(request);
            }
            catch (ArgumentException e)
            {
                OnError?.Invoke(e);
                await bot.SendTextMessageAsync(message.Chat.Id, e.Message);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }
        }

        private async void BotOnCallbackQueryReceived(object sender,
            CallbackQueryEventArgs callbackQueryEventArgs)
        {
            try
            {
                var request = AnalyzeIncomingCallback(callbackQueryEventArgs);
                await HandleIncomingRequest(request);
            }
            catch (ArgumentException e)
            {
                OnError?.Invoke(e);
                await bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, e.Message);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }
        }

        private async Task HandleIncomingRequest(IRequest request)
        {
            OnRequest?.Invoke(request);
            var response = requestHandler.GetResponse(request);
            await TelegramResponseHandler.SendResponse(bot, request.Author.TelegramId, response);
        }

        private IRequest AnalyzeIncomingMessage(Message message)
        {
            if (message.Type != MessageType.Text)
                throw new ArgumentException($"{message.Type} is not supported yet =[");
            var sender = message.Chat;
            var author = new Author(sender.Id, sender.FirstName, sender.LastName, sender.Username);
            return new BaseRequest(author, message.Text, message.Chat.Id);
        }

        private IRequest AnalyzeIncomingCallback(CallbackQueryEventArgs callback)
        {
            var sender = callback.CallbackQuery.Message.Chat;
            var author = new Author(sender.Id, sender.FirstName, sender.LastName, sender.Username);
            return new BaseRequest(author, callback.CallbackQuery.Data, sender.Id);
        }
    }
}