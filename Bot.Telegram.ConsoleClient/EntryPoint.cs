﻿using System;
using System.IO;
using Bot.Telegram.Common;
using Bot.Telegram.Common.Credentials;
using Newtonsoft.Json;

namespace Bot.Telegram.ConsoleClient
{
    internal static class EntryPoint
    {
        private static void Main()
        {
            var telegramCredentials =
                JsonConvert.DeserializeObject<TelegramCredentials>(
                    File.ReadAllText("Credentials/TelegramCredentials.json"));
            var tgBot = new TgBot(telegramCredentials);

            tgBot.OnRequest += request =>
            {
                var sender = request.Author;
                Console.WriteLine(
                    $"from: {sender.FirstName} {sender.LastName} {sender.TelegramId} text: {request.Command}");
            };

            tgBot.Start();
            Console.ReadLine();
            tgBot.Stop();
        }
    }
}