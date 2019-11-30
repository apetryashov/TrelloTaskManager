using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common.Commands
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
            return new CommandResponse( new TextResponse("не найдено", SessionStatus.Close));
        }
    }
}