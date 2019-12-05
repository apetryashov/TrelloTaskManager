using System;
using TaskManager.Bot.Telegram.Model.Session;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskManager.Bot.Telegram.Model
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