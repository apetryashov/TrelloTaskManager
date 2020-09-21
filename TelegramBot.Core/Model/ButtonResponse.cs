using System;

namespace TelegramBot.Core.Model
{
    public class ButtonResponse : IResponse
    {
        public ButtonResponse(string text, string[] buttons)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
        }

        public string[] Buttons { get; }
        public string Text { get; }
    }
}