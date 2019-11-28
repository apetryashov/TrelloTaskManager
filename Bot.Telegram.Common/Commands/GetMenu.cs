using Bot.Telegram.Common.Model;

namespace Bot.Telegram.Common.Commands
{
    public class GetMenu : ICommand
    {
        private readonly string[][] menu =
        {
            new[]
            {
                "все активные задачи",
                "добавить задачу"
            },
            new[]
            {
                "все сделанные задачи (в разработке)",
                "статистика по задачам (в разработке)"
            },
        };

        public string CommandTrigger => "/start";
        public ICommandResponse StartCommand(ICommandInfo commandInfo)
        {
            var response = new ButtonResponse("сделай правильный выбор", menu);
            return new CommandResponse(response);
        }
    }
}