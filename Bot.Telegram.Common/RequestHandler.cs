using System;
using Bot.Telegram.Common.Commands;
using Bot.Telegram.Common.Model;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using TaskManager.Common;

namespace Bot.Telegram.Common
{
    public class RequestHandler : IRequestHandler
    {
        private const string DefaultCommand = "/start";
        private readonly ICommand[] commands;
        private readonly InMemorySessionStorage sessionStorage = new InMemorySessionStorage();

        public RequestHandler(ITaskProvider taskProvider)
        {
            commands = new ICommand[]
            {
                new GetMenu(),
                new GetInactiveTaskList(taskProvider),
                new AddTask(taskProvider),
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
            var commandResponse = command.StartCommand(author, commandText, session);
            HandleCommandSession(author, session.CommandId, commandResponse.Session);

            return commandResponse.Response;
        }

        private IResponse Execute(Author author, string commandText)
        {
            var (command, commandIndex) = GetCommandByPrefix(commandText);
            var commandResponse = command.StartCommand(author);
            HandleCommandSession(author, commandIndex, commandResponse.Session);

            return commandResponse.Response;
        }

        private (ICommand command, int commandIndex) GetCommandByPrefix(string textCommand)
        {
            for (var index = 0; index < commands.Length; index++)
            {
                if (commands[index].CommandTrigger.StartsWith(textCommand))
                    return (commands[index], index);
            }

            return GetCommandByPrefix(DefaultCommand);
        }

        private void HandleCommandSession(Author author, int commandIndex, ICommandSession session)
        {
            var status = session.SessionStatus;
            if (status == SessionStatus.Expect)
            {
                if (session.ContinueIndex.HasValue)
                {
                    sessionStorage.AddUserSession(
                        author,
                        new Session(commandIndex, session.ContinueIndex.Value)
                    );
                }
                else
                {
                    throw new Exception(); // fix it
                }
            }
            else if (status == SessionStatus.Close)
            {
                sessionStorage.KillUserSession(author);
            }
        }
    }
}