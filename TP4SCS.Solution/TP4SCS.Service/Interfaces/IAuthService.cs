using TP4SCS.Library.Models.Data;

namespace TP4SCS.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(Account account);
    }
}
