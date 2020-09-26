using System.Threading.Tasks;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Model;

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

        public Task<IResponse> StartCommand(ICommandInfo commandInfo) => TextResponse
            .Create(message)
            .RunAsTask();
    }
}