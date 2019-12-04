using System;
using Bot.Telegram.Common.Model.Session;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Telegram.Common.Model
{
    public class ButtonResponse : IResponse
    {
        public ButtonResponse(string text, ReplyKeyboardMarkup buttons, SessionStatus sessionStatus)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
            SessionStatus = sessionStatus;
        }

        public ReplyKeyboardMarkup Buttons { get; }
        public string Text { get; }
        public SessionStatus SessionStatus { get; }
    }
}