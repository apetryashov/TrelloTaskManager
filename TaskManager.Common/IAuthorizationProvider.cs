using System.Threading.Tasks;

namespace TaskManager.Common
{
    public interface IAuthorizationProvider
    {
        Task<bool> IsValidAuthorizationToken(string userToken);
        Task CheckOrInitializeWorkspace(string userToken);
        string GetAuthorizationUrl();
    }
}