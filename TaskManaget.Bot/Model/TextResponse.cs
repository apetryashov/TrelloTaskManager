using System;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Model
{
    public class TextResponse : IResponse
    {
        private TextResponse(string responseText, SessionStatus sessionStatus)
        {
            if (responseText.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = responseText;
            SessionStatus = sessionStatus;
        }

        public string Text { get; }
        public SessionStatus SessionStatus { get; }
        
        public static IResponse ExpectedCommand(string text) => new TextResponse(text, SessionStatus.Expect);
        public static IResponse CloseCommand(string text) => new TextResponse(text, SessionStatus.Close);
        public static IResponse AbortCommand(string text) => new TextResponse(text, SessionStatus.Abort);

        public IResponse AsButton(string[][] keyboard)
        {
            return new ButtonResponse(Text, keyboard, SessionStatus);
        }
    }
}