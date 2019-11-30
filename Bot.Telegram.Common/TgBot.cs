using System;
using Bot.Telegram.Common.Credentials;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Storage;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Telegram.Common
{
    public class TgBot
    {
        private readonly ITelegramBotClient bot;
        private readonly IRequestHandler requestHandler;
        private readonly ITaskProvider taskProvider;

        public TgBot(TelegramCredentials credentials)
        {
            bot = new TelegramBotClient(credentials.AccessToken);
            taskProvider = new MoqTaskProvider();
            requestHandler = new RequestHandler(taskProvider);
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
                var botData = TelegramResponseHandler.ResponseAnalyzer(response);

                await bot.SendTextMessageAsync(message.Chat.Id, botData.Text, replyMarkup: botData.Markup);
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