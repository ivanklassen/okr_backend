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
        public async Task<IActionResult> changeUserRole([FromBody] ChangeUserRoleModel role, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userRole = await _context.Users.FirstOrDefaultAsync(p => p.Id.ToString() == idUser);


            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (role.role == Role.Student)
            {
                if (userRole.isAdmin == false && userRole.isDean == false) return Forbid();

                user.isStudent = !user.isStudent;
            }

            if (role.role == Role.Teacher)
            {
                if (userRole.isAdmin == false && userRole.isDean == false) return Forbid();

                user.isTeacher = !user.isTeacher;
            }

            if (role.role == Role.Dean)
            {
                if (userRole.isAdmin == false) return Forbid();

                user.isDean = !user.isDean;
            }

            if (role.role == Role.Admin)
            {
                if (userRole.isAdmin == false) return Forbid();

                user.isAdmin = !user.isAdmin;
            }

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("users")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserShortModel>))]
        public async Task<List<UserShortModel>> getAllUsers()
        {

            return await _context.Users.Select(p => new UserShortModel
            {
                Id = p.Id,
                surname = p.surname,
                name = p.name
            }).
            ToListAsync();
        }
    }
}
