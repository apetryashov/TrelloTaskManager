using System;
using System.Linq;
using TaskManager.Bot.Telegram.Model.Session;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaskManager.Bot.Telegram.Model
{
    public class InlineButtonResponse : IResponse
    {
        private InlineButtonResponse(
            string text,
            InlineKeyboardMarkup buttons,
            SessionStatus sessionStatus)
        {
            if (text.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = text;
            Buttons = buttons;
            SessionStatus = sessionStatus;
        }

        public InlineKeyboardMarkup Buttons { get; }
        public string Text { get; }
        public SessionStatus SessionStatus { get; }

        private InlineKeyboardMarkup AsInlineKeyboardMarkup((string text, string callback)[] buttons)
        {
            return buttons.Select(
                    button => new[] {InlineKeyboardButton.WithCallbackData(button.text, button.callback)})
                .ToArray();
        }

        public static InlineButtonResponse CreateWithHorizontalButtons(
            string text,
            (string text, string callback)[] buttons,
            SessionStatus sessionStatus)
        {
            var inlineButtons = buttons.Select(
                    button => InlineKeyboardButton.WithCallbackData(button.text, button.callback))
                .ToArray();

            return new InlineButtonResponse(text, inlineButtons, sessionStatus);
        }
        
        public static InlineButtonResponse CreateWithVerticalButtons(
            string text,
            (string text, string callback)[] buttons,
            SessionStatus sessionStatus)
        {
            var inlineButtons = buttons.Select(
                    button => new[] {InlineKeyboardButton.WithCallbackData(button.text, button.callback)})
                .ToArray();

            return new InlineButtonResponse(text, inlineButtons, sessionStatus);
        }
    }
}