using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Telegram.Common.Model
{
    public class ButtonResponse : IResponse
    {
        public ButtonResponse(string text, ReplyKeyboardMarkup buttons)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
        }

        public ReplyKeyboardMarkup Buttons { get; }
        public string Text { get; }
    }
}