using System;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Model
{
    public class ButtonResponse : IResponse
    {
        public ButtonResponse(string text, string[][] buttons, SessionStatus sessionStatus)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
            SessionStatus = sessionStatus;
        }

        public string[][] Buttons { get; }
        public string Text { get; }
        public SessionStatus SessionStatus { get; }
    }
}