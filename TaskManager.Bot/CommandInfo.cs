using TaskManager.Common.Domain;

namespace TaskManager.Bot
{
    public class CommandInfo : ICommandInfo
    {
        public CommandInfo(Author author, string command)
        {
            Author = author;
            Command = command;
        }

        public Author Author { get; }
        public string Command { get; }
    }
}