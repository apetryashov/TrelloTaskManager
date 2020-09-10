using TaskManager.Bot.Model;

namespace TaskManager.Bot.Commands
{
    public class TextCommand : ICommandWithPrefixValidation
    {
        private readonly string message;

        protected TextCommand(
            string commandTrigger,
            string message)
        {
            this.message = message;
            CommandTrigger = commandTrigger;
        }

        public string CommandTrigger { get; }

        public IResponse StartCommand(ICommandInfo commandInfo) => TextResponse.Create(message);
    }
}