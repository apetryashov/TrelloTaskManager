using TaskManager.Bot.Model;

namespace TaskManager.Bot.Commands
{
    public class TextCommand : ICommand
    {
        private readonly string message;

        protected TextCommand(
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

        public IResponse StartCommand(ICommandInfo commandInfo) => TextResponse.Create(message);
    }
}