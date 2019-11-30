using Bot.Telegram.Common.Commands;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using Bot.Telegram.Common.Storage;

namespace Bot.Telegram.Common
{
    public class RequestHandler : IRequestHandler
    {
        private const string DefaultCommand = "/start";
        private readonly ICommand[] commands;
        private readonly ISessionStorage sessionStorage = new InMemorySessionStorage();

        public RequestHandler(ITaskProvider taskProvider)
        {
            commands = new ICommand[]
            {
                new GetMenu(),
                new GetInactiveTaskList(taskProvider),
                new AddTask(taskProvider),
                new GetTaskInfo(taskProvider)
            };
        }

        public IResponse GetResponse(IRequest request)
        {
            var author = request.Author;
            var commandText = request.Command;

            return sessionStorage.TryGetUserSession(author, out var session)
                ? Execute(author, commandText, session)
                : Execute(author, commandText);
        }

        private IResponse Execute(Author author, string commandText, ISession session)
        {
            var command = commands[session.CommandId];
            var commandInfo = new CommandInfo(author, commandText, session);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, session.CommandId, commandResponse.Session);

            return commandResponse.Response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            var (command, commandIndex) = GetCommandByPrefix(commandText);
            var commandInfo = new CommandInfo(author, commandText);
            var commandResponse = command.StartCommand(commandInfo);
            sessionStorage.HandleCommandSession(author, commandIndex, commandResponse.Session);

            return commandResponse.Response;
        }

        private (ICommand command, int commandIndex) GetCommandByPrefix(string textCommand)
        {
            for (var index = 0; index < commands.Length; index++)
            {
                if (textCommand.StartsWith(commands[index].CommandTrigger))
                    return (commands[index], index);
            }

            return GetCommandByPrefix(DefaultCommand);
        }
    }
}