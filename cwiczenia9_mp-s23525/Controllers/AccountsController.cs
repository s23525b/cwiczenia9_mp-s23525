using cwiczenia9_mp_s23525.Models.DTO;
using cwiczenia9_mp_s23525.Services.AccountsService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cwiczenia9_mp_s23525.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsDbService _accountsDbService;
        public AccountsController(IAccountsDbService idbService)
        {
            _accountsDbService = idbService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            if (await _accountsDbService.IfUserExists(request.Login)) return BadRequest("[ERROR] User already exists");
            await _accountsDbService.RegisterUser(request);
            return Ok($"Registered new user login: {request.Login}");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterRequest request)
        {
            if (!await _accountsDbService.IfUserExists(request.Login)) return NotFound("[ERROR] User not found");

            var result = await _accountsDbService.Login(request);
            if(result != null)
                return Ok(result);

            return BadRequest("[ERROR] Something went wrong");
        }
        [HttpPost("newToken")]
        public async Task<IActionResult> GetNewFreshAccessToken(string refreshToken)
        {
            var result = await _accountsDbService.GetNewAccessToken(refreshToken);
            if (result == null) return BadRequest("[ERROR] Something went wrong");

            return Ok(result.AccessToken);
        }
    }
}
