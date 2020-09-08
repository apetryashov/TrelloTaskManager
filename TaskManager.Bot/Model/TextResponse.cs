using System;

namespace TaskManager.Bot.Model
{
    public class TextResponse : IResponse
    {
        private TextResponse(string responseText)
        {
            if (responseText.Length == 0)
                throw new ArgumentException("Empty response text");
            Text = responseText;
        }

        public string Text { get; }

        public static TextResponse Create(string responseText) => new TextResponse(responseText);

        public IResponse AsButton(string[][] keyboard) => new ButtonResponse(Text, keyboard);
    }
}