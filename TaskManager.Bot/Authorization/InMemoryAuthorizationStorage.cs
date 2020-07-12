using System.Collections.Generic;
using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Authorization
{
    public class InMemoryAuthorizationStorage : IAuthorizationStorage
    {
        private readonly Dictionary<long, string> memory = new Dictionary<long, string>();

        public bool IsAuthorizedUser(Author author) => memory.ContainsKey(author.TelegramId);

        public string GetUserToken(Author author) => memory[author.TelegramId];

        public void SetUserToken(Author author, string token) => memory.Add(author.TelegramId, token);
    }
}