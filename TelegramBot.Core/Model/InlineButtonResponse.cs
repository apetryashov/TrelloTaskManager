using System;
using System.Linq;

namespace TelegramBot.Core.Model
{
    public class InlineButtonResponse : IResponse
    {
        private InlineButtonResponse(
            string text,
            (string text, string callback)[][] buttons)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
        }

        public (string text, string callback)[][] Buttons { get; }
        public string Text { get; }

        public static InlineButtonResponse CreateWithHorizontalButtons(
            string text,
            (string text, string callback)[] buttons)
            => new InlineButtonResponse(text, new[] {buttons});

        public static InlineButtonResponse CreateWithVerticalButtons(
            string text,
            (string text, string callback)[] buttons)
        {
            var inlineButtons = buttons.Select(
                    button => new[] {button})
                .ToArray();

            return new InlineButtonResponse(text, inlineButtons);
        }
    }
}