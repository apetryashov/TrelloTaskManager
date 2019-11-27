using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common
{
    public interface ICommand
    {
        string CommandTrigger { get; }
        ICommandResponse StartCommand(Author author);
        ICommandResponse StartCommand(Author author, string commandText, ISession commandSession);
    }
}