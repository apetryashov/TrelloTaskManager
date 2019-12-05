using System;
using TaskManager.Bot.Telegram.Credentials;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Domain;
using TaskManager.Common.Storage;
using TaskManager.Common.Tasks;
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
            this.requestHandler =requestHandler;
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