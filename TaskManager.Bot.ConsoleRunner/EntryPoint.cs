using System;
using System.IO;
using Newtonsoft.Json;
using Ninject;
using TaskManager.Bot.Telegram;
using TaskManager.Bot.Telegram.Credentials;
using TaskManager.Ioc;
using TaskManaget.Bot;

namespace TaskManager.Bot.ConsoleRunner
{
    internal static class EntryPoint
    {
        private static void Main()
        {
            var telegramCredentials =
                JsonConvert.DeserializeObject<TelegramCredentials>(
                    File.ReadAllText("Credentials/TelegramCredentials.json"));

            var kernel = NinjectConfig.GetKernel(telegramCredentials);

            var tgBot = kernel.Get<IBot>();

            tgBot.OnRequest += request =>
            {
                var sender = request.Author;
                Console.WriteLine(
                    $"from: {sender.FirstName} {sender.LastName} {sender.TelegramId} text: {request.Command}");
            };

            tgBot.OnError += Console.WriteLine;
            
            tgBot.Start();
            Console.ReadLine();
            tgBot.Stop();
        }
    }
}