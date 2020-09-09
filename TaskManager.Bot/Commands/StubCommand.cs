namespace TaskManager.Bot.Commands
{
    public class StubCommand : TextCommand
    {
        public StubCommand(string commandTrigger)
            : base(commandTrigger, "Данная функция пока не реализована")
        {
        }
    }
}