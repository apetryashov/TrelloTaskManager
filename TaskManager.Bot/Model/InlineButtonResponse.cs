using System;
using System.Linq;
using TaskManager.Bot.Model.Session;

namespace TaskManager.Bot.Model
{
    public class InlineButtonResponse : IResponse
    {
        private InlineButtonResponse(
            string text,
            (string text, string callback)[][] buttons,
            SessionStatus sessionStatus)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
            SessionStatus = sessionStatus;
        }

        public (string text, string callback)[][] Buttons { get; }
        public string Text { get; }
        public SessionStatus SessionStatus { get; }

        public static InlineButtonResponse CreateWithHorizontalButtons(
            string text,
            (string text, string callback)[] buttons,
            SessionStatus sessionStatus)
        {
            return new InlineButtonResponse(text, new[] {buttons}, sessionStatus);
        }

        public static InlineButtonResponse CreateWithVerticalButtons(
            string text,
            (string text, string callback)[] buttons,
            SessionStatus sessionStatus)
        {
            var inlineButtons = buttons.Select(
                    button => new[] {button})
                .ToArray();

            return new InlineButtonResponse(text, inlineButtons, sessionStatus);
        }
    }
}