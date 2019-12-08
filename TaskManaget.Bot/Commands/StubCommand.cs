using TaskManaget.Bot.Model;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Commands
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
            return new CommandResponse(TextResponse.CloseCommand("Данная функция пока не реализована"));
        }
    }
}