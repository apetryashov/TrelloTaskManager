using TaskManager.Bot.Telegram.Model.Domain;
using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram
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