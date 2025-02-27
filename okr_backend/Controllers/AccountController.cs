using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using okr_backend.Models;
using okr_backend.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace okr_backend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //[HttpPost("registration")]
        //public async Task<IActionResult> Register([FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    if (user.password != user.confirmPassword)
        //    {
        //        return BadRequest();
        //    }

        //    user.Id = Guid.NewGuid();
        //    user.isTeacher = false;
        //    user.isStudent = false;
        //    user.isAdmin = false;

        //    await _context.AddAsync(user);
        //    await _context.SaveChangesAsync();

        //    var token = GenerateJwtToken(user);
        //    return Ok(token);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(p => p.email == loginModel.email);

            if (user == null || user.password != loginModel.password)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtTokenLogin(user.Id, user.fullName, user.email);
            return Ok(new AuthResponse { Token = token });
        }

        //private string GenerateJwtToken(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.fullName),
        //        new Claim(ClaimTypes.Email, user.email)
        //    };

        //    var jwtToken = new JwtSecurityToken(
        //        expires: DateTime.UtcNow.AddHours(2),
        //        claims: claims,
        //        signingCredentials:
        //        new SigningCredentials(
        //            new SymmetricSecurityKey(
        //                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256));

        //    return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        //}

        private string GenerateJwtTokenLogin(Guid id, string fullName, string email)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, email)
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(2),
                claims: claims,
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> logout()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            
            if (string.IsNullOrEmpty(authHeader)) return BadRequest();
            
            string token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token)) return BadRequest();

            if (checkToken(token)) return Unauthorized();

            var Token = new BannedToken { bannedToken = token };

            await _context.AddAsync(Token);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool checkToken(string token)
        {
            var bannedToken = _context.BannedTokens.FirstOrDefault(p => p.bannedToken == token);

            if (bannedToken == null)
            {
                return false;
            }
            return true;
        }
    }
}
