namespace TaskManager.Bot.Commands
{
    public class HelpCommand : TextCommand
    {
        private const string HelpMessage = "help-message";
        
        public HelpCommand()
            : base(
                "/help", 
                HelpMessage)
        {
        }
    }
}