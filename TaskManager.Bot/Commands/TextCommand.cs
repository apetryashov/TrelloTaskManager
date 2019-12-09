using TaskManager.Bot.Model;

namespace TaskManager.Bot.Commands
{
    public class TextCommand : ICommand
    {
        private readonly string message;

        public TextCommand(
            string commandTrigger,
            string message,
            bool isPublicCommand = true)
        {
            this.message = message;
            CommandTrigger = commandTrigger;
            IsPublicCommand = isPublicCommand;
        }

        public bool IsPublicCommand { get; }
        public string CommandTrigger { get; }

        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            return new CommandResponse(TextResponse.CloseCommand(message));
        }
    }
}