using System;
using System.Threading.Tasks;
using TaskManager.Common.Tasks;
using TelegramBot.Core.Commands;
using TelegramBot.Core.Model;

namespace TaskManager.Bot.Commands
{
    public class AddTask : ICommand
    {
        private const char TelegramNewLineChar = '\n';
        private readonly ITaskHandler taskProvider;

        public AddTask(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public async Task<IResponse> StartCommand(ICommandInfo commandInfo)
        {
            var task = ExtractTaskInfo(commandInfo);
            var fullTask = await taskProvider.AddNewTask(commandInfo.Author.TelegramId, task);

            return TextResponse.Create(@$"
Задача успешно добавлена!

{fullTask}
");
        }

        private static MyTask ExtractTaskInfo(ICommandInfo commandInfo)
        {
            var nameAndDescriptions =
                commandInfo.Command.Split(TelegramNewLineChar, 2, StringSplitOptions.RemoveEmptyEntries);

            return new MyTask
            {
                Name = nameAndDescriptions[0],
                Description = nameAndDescriptions.Length == 2 ? nameAndDescriptions[1] : null
            };
        }
    }
}