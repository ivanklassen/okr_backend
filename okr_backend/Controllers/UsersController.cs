using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using okr_backend.Models;
using okr_backend.Persistence;
using System.Security.Claims;

namespace okr_backend.Controllers
{
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UsersController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("roles")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRolesModel))]
        public async Task<IActionResult> getRoles()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id.ToString() == id);

            UserRolesModel roles = new UserRolesModel();

            roles.isTeacher = user.isTeacher;
            roles.isStudent = user.isStudent;
            roles.isAdmin = user.isAdmin;
            roles.isDean = user.isDean;

            return Ok(roles);
        }

        [HttpPost("user/{id}/role")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        public IActionResult changeUserRole([FromBody] ChangeUserRoleModel role, Guid id)
        {
            UserModel user = new UserModel();

            //some code

            return Ok(user);
        }

        [HttpGet("users")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserShortModel>))]
        public IActionResult getAllUsers()
        {
            List<UserShortModel> users = new List<UserShortModel>();

            //some code

            return Ok(users);
        }
    }
}
