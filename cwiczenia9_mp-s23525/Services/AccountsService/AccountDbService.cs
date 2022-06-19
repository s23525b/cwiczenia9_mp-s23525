using cwiczenia9_mp_s23525.Models;
using cwiczenia9_mp_s23525.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Services.AccountsService
{
    public class AccountDbService : IAccountsDbService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IConfiguration _configuration;
        public AccountDbService(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _mainDbContext = mainDbContext;
            _configuration = configuration;
        }

        public string GetAccessToken()
        {
            var claims= new[]
            {
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.Role, "client")
            };
            
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken("https://localhost:5001", "https://localhost:5001",
                claims, expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<TokensDto> GetNewAccessToken(string refreshToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);
            if (user == null) return null;
            return user.RefreshTokenExpiration > DateTime.Now ? new TokensDto { AccessToken = GetAccessToken() } : null;
        }

        public string GetRefreshToken()
        {
            using var randomNumber = RandomNumberGenerator.Create();
            var data = new byte[512];
            randomNumber.GetBytes(data);
            var refreshToken = Convert.ToBase64String(data);
            return refreshToken;
        }

        public async Task<bool> IfUserExists(string login)
        {
            return await _mainDbContext.Users.Where(e => e.Login == login).AnyAsync();
        }

        public async Task<TokensDto> Login(RegisterRequest request)
        {
            var newRefreshToken = GetRefreshToken();
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(e => e.Login == request.Login);
            var check = new PasswordHasher<RegisterRequest>().VerifyHashedPassword(request, user.Password, request.Password);
            if (PasswordVerificationResult.Success != check) return null;
            if (!(user.RefreshTokenExpiration < DateTime.Now))
                return new TokensDto { AccessToken = GetAccessToken(), RefreshToken = user.RefreshToken };
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddHours(1);

            _mainDbContext.Update(user);
            await _mainDbContext.SaveChangesAsync();
            return new TokensDto { AccessToken = GetAccessToken(), RefreshToken = newRefreshToken };
        }

        public async Task RegisterUser(RegisterRequest request)
        {
            var user = new User()
            
            {
                Login = request.Login,
                RefreshToken = GetRefreshToken(),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(0.1)
            };
            
            var passwordHasher = new PasswordHasher<RegisterRequest>();
            var userPassword = passwordHasher.HashPassword(request, request.Password);
            user.Password = userPassword;
            _mainDbContext.Add(user);
            await _mainDbContext.SaveChangesAsync();
        }
    }
}
