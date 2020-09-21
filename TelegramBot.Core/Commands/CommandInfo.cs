using TelegramBot.Core.Domain;

namespace TelegramBot.Core.Commands
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