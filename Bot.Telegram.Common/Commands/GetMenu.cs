using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

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

        public ICommandResponse StartCommand(Author author)
        {
            var response = new ButtonResponse("сделай правильный выбор", menu);
            return new CommandResponse(response);
        }

        public ICommandResponse StartCommand(Author author, string commandText, ISession commandSession)
        {
            throw new System.NotImplementedException();
        }
    }
}