using System;
using TaskManager.Bot.Telegram.Credentials;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Domain;
using TaskManager.Common.Storage;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaskManager.Bot.Telegram
{
    public class TgBot
    {
        private readonly ITelegramBotClient bot;
        private readonly IRequestHandler requestHandler;
        private readonly ITaskProvider taskProvider;

        public TgBot(TelegramCredentials credentials)
        {
            bot = new TelegramBotClient(credentials.AccessToken);
            taskProvider = null;
            requestHandler = new RequestHandler(taskProvider, credentials.AppKey);
        }

        public event Action<IRequest> OnRequest;
        public event Action<Exception> OnError;

        public void Start()
        {
            bot.StartReceiving();
            bot.OnMessage += BotOnMessageReceived;
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
                OnRequest?.Invoke(request);
                var response = requestHandler.GetResponse(request);
                await TelegramResponseHandler.SendResponse(bot, message.Chat.Id, response);
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

        private IRequest AnalyzeIncomingMessage(Message message)
        {
            if (message.Type != MessageType.Text)
                throw new ArgumentException($"{message.Type} is not supported yet =[");
            var sender = message.Chat;
            var author = new Author(sender.Id, sender.FirstName, sender.LastName, sender.Username);
            return new BaseRequest(author, message.Text, message.Chat.Id);
        }
    }
}