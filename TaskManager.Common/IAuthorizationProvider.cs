using System;
using System.Threading.Tasks;
using TelegramBot.Core.Domain;

namespace TaskManager.Common
{
    public interface IAuthorizationProvider
    {
        Task<bool> IsValidAuthorizationToken(string userToken);
        Task CheckOrInitializeWorkspace(string userToken);
        Uri GetAuthorizationUrl(Author author);
    }
}