using System.Collections.Generic;
using TaskManaget.Bot.Model.Domain;

namespace TaskManaget.Bot.Authorization
{
    public class InMemoryAuthorizationStorage : IAuthorizationStorage
    {
        private readonly Dictionary<long, string> memory = new Dictionary<long, string>();

        public bool IsAuthorizedUser(Author author)
        {
            return memory.ContainsKey(author.TelegramId);
        }

        public string GetUserToken(Author author)
        {
            return memory[author.TelegramId];
        }

        public void SetUserToken(Author author, string token)
        {
            memory.Add(author.TelegramId, token);
        }
    }
}