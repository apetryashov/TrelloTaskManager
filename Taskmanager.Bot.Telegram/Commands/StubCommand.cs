using System.ComponentModel.DataAnnotations;
using TaskManager.Bot.Telegram.Model;
using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram.Commands
{
    public class StubCommand : ICommand
    {
        public StubCommand(string commandTrigger, bool isPublicCommand = true)
        {
            CommandTrigger = commandTrigger;
            IsPublicCommand = isPublicCommand;
        }

        public bool IsPublicCommand { get; }
        public string CommandTrigger { get; }

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            return new CommandResponse(new TextResponse("Данная функция пока не реализована", SessionStatus.Close));
        }
    }
}