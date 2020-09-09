namespace TaskManager.Bot
{
    public interface ICommandWithPrefixValidation : ICommand
    {
        string CommandTrigger { get; }
        bool IsValidCommand(string commandText) => CommandTrigger.StartsWith(commandText);
    }
}