namespace TaskManager.Bot.Commands
{
    public class StubCommand : TextCommand
    {
        public StubCommand(string commandTrigger, bool isPublicCommand = true)
            : base(commandTrigger, "Данная функция пока не реализована", isPublicCommand)
        {
        }
    }
}