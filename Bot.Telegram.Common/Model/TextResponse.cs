using System;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common.Model
{
    public class TextResponse : IResponse
    {
        public TextResponse(string responseText)
        {
            if (responseText.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = responseText;
        }

        public string Text { get; }
    }
}