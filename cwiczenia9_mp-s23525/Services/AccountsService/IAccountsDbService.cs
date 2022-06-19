using cwiczenia9_mp_s23525.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.AccountsService
{
    public interface IAccountsDbService
    {
        Task RegisterUser(RegisterRequest request);
        Task<bool> IfUserExists(string login);
        Task<TokensDto> Login(RegisterRequest request);
        Task<TokensDto> GetNewAccessToken(string refreshToken);
        string GetRefreshToken();
        string GetAccessToken();
    }
}
