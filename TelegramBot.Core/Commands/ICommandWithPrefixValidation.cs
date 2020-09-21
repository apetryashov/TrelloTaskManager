namespace TelegramBot.Core.Commands
{
    public interface ICommandWithPrefixValidation : ICommand
    {
        string CommandTrigger { get; }
        bool IsValidCommand(string commandText) => commandText.StartsWith(CommandTrigger);
    }
}