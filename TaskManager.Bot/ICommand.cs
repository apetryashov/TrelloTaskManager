namespace TaskManager.Bot
{
    public interface ICommand : IDefaultCommand
    {
        bool IsPublicCommand { get; }
        string CommandTrigger { get; }
    }
}