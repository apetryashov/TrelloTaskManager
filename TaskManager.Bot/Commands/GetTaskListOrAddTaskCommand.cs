using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IResponse> StartCommand(ICommandInfo commandInfo)
        {
            var columns = await textButtonMenuProvider.GetButtons(commandInfo.Author.TelegramId);
            var command = commandInfo.Command;
            if (!columns.Contains(command))
                return await addTaskCommand.StartCommand(commandInfo);

            var tasks = await taskProvider.GetAllColumnTasks(commandInfo.Author.TelegramId, command);
            var buttons = tasks.Select(x => (x.Name, $"/task_{x.Id}")).ToArray();
            return InlineButtonResponse.CreateWithVerticalButtons(command, buttons);
        }
    }
}