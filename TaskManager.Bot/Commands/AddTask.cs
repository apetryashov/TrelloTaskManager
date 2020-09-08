using System;
using TaskManager.Bot.Model;
using TaskManager.Common.Tasks;

namespace TaskManager.Bot.Commands
{
    public class AddTask : IDefaultCommand
    {
        private const char TelegramNewLineChar = '\n';
        private readonly ITaskHandler taskProvider;

        public AddTask(ITaskHandler taskProvider) => this.taskProvider = taskProvider;

        public IResponse StartCommand(ICommandInfo commandInfo)
        {
            var task = ExtractTaskInfo(commandInfo);
            var fullTask = taskProvider.AddNewTask(commandInfo.Author.UserToken, task).Result;

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