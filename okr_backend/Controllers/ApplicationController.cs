using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using okr_backend.Models;
using okr_backend.Persistence;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace okr_backend.Controllers
{
    [ApiController]
    [Authorize]
    public class ApplicationController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ApplicationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize]
        [HttpGet("application")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FullApplicationModel>))]
        public async Task<IActionResult> getApplications()
        {
            List<FullApplicationModel> applications = _context.Applications.Include(p => p.extensions)
                .Select(p => new FullApplicationModel
                {
                    Id = p.Id,
                    userId = p.userId,
                    fromDate = p.fromDate,
                    toDate = p.toDate,
                    description = p.description,
                    image = p.image,
                    status = p.status,
                    comment = p.comment,
                    extensions = p.extensions.Select(p => new ExtensionApplicationModel
                    {
                        Id = p.Id,
                        applicationId = p.applicationId,
                        extensionToDate = p.extensionToDate,
                        description = p.description,
                        image = p.image,
                        status = p.status,
                        comment = p.comment,
                    }).ToList()
                })
                .ToList();


            return Ok(applications);
        }

        [Authorize]
        [HttpPost("application")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationModel))]
        public async Task<IActionResult> createApplication([FromBody] CreateApplicationModel model)
        {
            var idstr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Guid id;
            Guid.TryParse(idstr, out id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Models.Application application = new Models.Application();
            ApplicationModel app = new ApplicationModel();

            application.Id = Guid.NewGuid();
            application.userId = id;
            application.fromDate = model.fromDate;
            application.toDate = model.toDate;
            application.description = model.description;
            application.image = model.image;
            application.status = Status.inProcess;

            await _context.AddAsync(application);
            await _context.SaveChangesAsync();

            app.Id = application.Id;
            app.userId = id;
            app.fromDate = model.fromDate;
            app.toDate = model.toDate;
            app.description = model.description;
            app.image = model.image;
            app.status = Status.inProcess;

            return Ok(app);
        }

        [Authorize]
        [HttpPut("application/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationModel))]
        public async Task<IActionResult> editApplication([FromBody] CreateApplicationModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var app = await _context.Applications.FirstOrDefaultAsync(p => p.Id == id);

            app.fromDate = model.fromDate;
            app.toDate = model.toDate;
            app.description = model.description;
            app.image = model.image;

            await _context.SaveChangesAsync();

            ApplicationModel application = new ApplicationModel();

            application.Id = app.Id;
            application.userId = app.userId;
            application.fromDate = model.fromDate;
            application.toDate = model.toDate;
            application.description = model.description;
            application.image = model.image;
            application.status = app.status;

            return Ok(application);
        }

        [Authorize]
        [HttpDelete("application/{id}")]
        public async Task<IActionResult> deleteApplication(Guid id)
        {

            await _context.Applications.Where(p => p.Id == id).ExecuteDeleteAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("application/my")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FullApplicationModel>))]
        public async Task<IActionResult> getMyApplications()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<FullApplicationModel> applications = _context.Applications.Where(p => p.userId.ToString() == id).Include(p => p.extensions)
                .Select(p => new FullApplicationModel
                {
                    Id = p.Id,
                    userId = p.userId,
                    fromDate = p.fromDate,
                    toDate = p.toDate,
                    description = p.description,
                    image = p.image,
                    status = p.status,
                    comment = p.comment,
                    extensions = p.extensions.Select(p => new ExtensionApplicationModel
                    {
                        Id = p.Id,
                        applicationId = p.applicationId,
                        extensionToDate = p.extensionToDate,
                        description = p.description,
                        image = p.image,
                        status = p.status,
                        comment = p.comment,
                    }).ToList()
                })
                .ToList();


            return Ok(applications);

        }

        [Authorize]
        [HttpPost("application/{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FullApplicationModel))]
        public async Task<IActionResult> EditStatusApplication([FromBody] ChangeStatusApplication status, Guid id)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(p => p.Id == id);

            if (status.status == Status.Accepted)
            {
                app.status = Status.Accepted;
            }
            if (status.status == Status.Rejected)
            {
                app.status = Status.Rejected;
            }

            app.comment = status.comment;

            await _context.SaveChangesAsync();

            FullApplicationModel model = _context.Applications.Where(p => p.Id == id).Include(p => p.extensions)
                .Select(p => new FullApplicationModel
                {
                    Id = p.Id,
                    userId = p.userId,
                    fromDate = p.fromDate,
                    toDate = p.toDate,
                    description = p.description,
                    image = p.image,
                    status = p.status,
                    comment = p.comment,
                    extensions = p.extensions.Select(p => new ExtensionApplicationModel
                    {
                        Id = p.Id,
                        applicationId = p.applicationId,
                        extensionToDate = p.extensionToDate,
                        description = p.description,
                        image = p.image,
                        status = p.status,
                        comment = p.comment,
                    }).ToList()
                })
                .FirstOrDefault();

            return Ok(model);
        }

    }
}
