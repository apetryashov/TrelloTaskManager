using System.Linq;
using TaskManager.Bot.Authorization;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class GetTaskListOrAddTaskCommand : ICommand
    {
        private readonly ITextButtonMenuProvider textButtonMenuProvider;
        private readonly ITaskHandler taskProvider;
        private readonly AddTask addTaskCommand;

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
            var columns = textButtonMenuProvider.GetButtons(commandInfo.Author);
            var command = commandInfo.Command;
            if (!columns.Contains(command))
                return addTaskCommand.StartCommand(commandInfo);

            var tasks = taskProvider.GetAllTasks(commandInfo.Author.UserToken, command).Result;
            var buttons = tasks.Select(x => (x.Name, $"/task_{x.Id}")).ToArray();
            return InlineButtonResponse.CreateWithVerticalButtons(command, buttons);
        }
    }
}