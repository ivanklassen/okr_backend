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

        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (model.password != model.confirmPassword)
            {
                return BadRequest();
            }

            var emailUser = await _context.Users.FirstOrDefaultAsync(p => p.email == model.email);

            if (emailUser != null)
            {
                return BadRequest();
            }

            User user = new User();

            user.surname = model.surname;
            user.name = model.name;
            user.patronymic = model.patronymic;
            user.email = model.email;
            user.password = model.password;

            user.Id = Guid.NewGuid();
            user.isTeacher = false;
            user.isStudent = true;
            user.isAdmin = false;
            user.isDean = false;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user.Id, user.surname, user.email);
            return Ok(new AuthResponse { Token = token });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
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

            var token = GenerateJwtToken(user.Id, user.surname, user.email);
            return Ok(new AuthResponse { Token = token });
        }

        private string GenerateJwtToken(Guid id, string fullName, string email)
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

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileModel))]
        public async Task<IActionResult> getCurrentUsersProfile()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id.ToString() == id);

            var profile = new UserProfileModel();
            profile.surname = user.surname;
            profile.email = user.email;
            profile.name = user.name;
            profile.patronymic = user.patronymic;

            return Ok(profile);
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileModel))]
        public async Task<IActionResult> editCurrentUsersProfile([FromBody] EditUserProfileModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id.ToString() == id);


            user.surname = model.surname;
            user.name = model.name;
            user.patronymic = model.patronymic;

            await _context.SaveChangesAsync();

            var profile = new UserProfileModel();
            profile.surname = user.surname;
            profile.name = user.name;
            profile.patronymic = user.patronymic;
            profile.email = user.email;

            return Ok(profile);
        }

        [HttpGet("profile/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileModel))]
        public async Task<IActionResult> getUsersProfileById(Guid id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            var profile = new UserProfileModel();
            profile.surname = user.surname;
            profile.email = user.email;
            profile.name = user.name;
            profile.patronymic = user.patronymic;

            return Ok(profile);
        }
    }
}
