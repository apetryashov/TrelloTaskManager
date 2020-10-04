namespace TaskManager.Trello
{
    public class TrelloApiToken
    {
        public string Token { get; set; }
        
        public static implicit operator string(TrelloApiToken token) => token.Token;
    }
}