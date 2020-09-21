using System.Linq;
using TaskManager.Common;
using TaskManager.Common.Tasks;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Model;

namespace TaskManager.Bot.Commands
{
    public class GetTaskListOrAddTaskCommand : ICommand
    {
        private readonly AddTask addTaskCommand;
        private readonly ITaskHandler taskProvider;
        private readonly ITextButtonMenuProvider textButtonMenuProvider;

        public GetTaskListOrAddTaskCommand(
            ITextButtonMenuProvider textButtonMenuProvider,
            ITaskHandler taskProvider,
            AddTask addTaskCommand)
        {
            this.textButtonMenuProvider = textButtonMenuProvider;
            this.taskProvider = taskProvider;
            this.addTaskCommand = addTaskCommand;
        }

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var columns = textButtonMenuProvider.GetButtons(commandInfo.Author.TelegramId);
            var command = commandInfo.Command;
            if (!columns.Contains(command))
                return addTaskCommand.StartCommand(commandInfo);

            var tasks = taskProvider.GetAllTasks(commandInfo.Author.TelegramId, command).Result;
            var buttons = tasks.Select(x => (x.Name, $"/task_{x.Id}")).ToArray();
            return InlineButtonResponse.CreateWithVerticalButtons(command, buttons);
        }
    }
}