using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common
{
    public class CommandInfo : ICommandInfo
    {
        public CommandInfo(Author author, string command, ISession session)
        {
            Author = author;
            Command = command;
            Session = session;
        }
        
        public CommandInfo(Author author, string command)
        {
            Author = author;
            Command = command;
        }

        public Author Author { get; }
        public string Command { get; }
        public ISession Session { get; } //is it really need here?
    }
}