using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common
{
    public class CommandInfo : ICommandInfo
    {
        public CommandInfo(Author author, string command, ISessionMeta meta)
        {
            Author = author;
            Command = command;
            SessionMeta = meta;
        }
        
        public CommandInfo(Author author, string command)
        {
            Author = author;
            Command = command;
        }

        public Author Author { get; }
        public string Command { get; }
        public ISessionMeta SessionMeta { get; }
    }
}