using db.dominio;
using db.Response;

namespace Core.service
{
    public interface IAuthService
    {
        Task<RsTrxService> validateLogin(Login model);
        Task<RsTrxService> rememberPassword(Login model);
        Task<RsTrxService> unlockUser(Login model);
        Task<RsTrxService> registerUser(Usuario model);
        Task<RsTrxService> updateUser(Usuario model);
        Task<IEnumerable<Usuario>> consultPersonForId(int id);
    }
}
