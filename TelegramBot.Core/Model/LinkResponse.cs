using System;

namespace TelegramBot.Core.Model
{
    public class LinkResponse : IResponse
    {
        public string LinkMessage;

        private LinkResponse(string responseText, string linkMessage, Uri link)
        {
            if (responseText.Length == 0)
                throw new ArgumentException("Empty response text");
            LinkMessage = linkMessage;
            Text = responseText;
            Link = link;
        }

        public string Text { get; }
        public Uri Link { get; }

        public static LinkResponse Create(string responseText, string linkMessage, Uri uri)
            => new LinkResponse(responseText, linkMessage, uri);
    }
}