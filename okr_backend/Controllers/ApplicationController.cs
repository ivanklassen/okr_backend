using Microsoft.AspNetCore.Mvc;
using okr_backend.Models;
using okr_backend.Persistence;

namespace okr_backend.Controllers
{
    [ApiController]
    public class ApplicationController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ApplicationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("application")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FullApplicationModel>))]
        public async Task<IActionResult> getApplications()
        {
            List<FullApplicationModel> applications = new List<FullApplicationModel>();

            // some code

            return Ok(applications);
        }

        [HttpPost("application")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationModel))]
        public async Task<IActionResult> createApplication([FromBody] CreateApplicationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationModel application = new ApplicationModel();

            // some code

            return Ok(application);
        }

        [HttpPut("application/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationModel))]
        public async Task<IActionResult> editApplication([FromBody] CreateApplicationModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationModel application = new ApplicationModel();

            // some code

            return Ok(application);
        }

        [HttpDelete("application/{id}")]
        public async Task<IActionResult> deleteApplication(Guid id)
        {

            // some code

            return Ok();
        }

        [HttpGet("application/my")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FullApplicationModel>))]
        public async Task<IActionResult> getMyApplications()
        {
            List<FullApplicationModel> applications = new List<FullApplicationModel>();

            // some code

            return Ok(applications);
        }

        [HttpPost("application/{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FullApplicationModel))]
        public async Task<IActionResult> EditStatusApplication([FromBody] ChangeStatusApplication status)
        {
            FullApplicationModel application = new FullApplicationModel();

            // some code

            return Ok(application);
        }

    }
}
