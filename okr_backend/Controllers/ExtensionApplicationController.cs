using Microsoft.AspNetCore.Mvc;
using okr_backend.Models;
using okr_backend.Persistence;

namespace okr_backend.Controllers
{
    [ApiController]
    public class ExtensionApplicationController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ExtensionApplicationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("extensionApplication/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExtensionApplicationModel))]
        public async Task<IActionResult> createExtension([FromBody] CreateExtensionApplicationModel model, Guid applicationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExtensionApplicationModel extension = new ExtensionApplicationModel();

            // some code

            return Ok(extension);
        }

        [HttpPut("extensionApplication/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExtensionApplicationModel))]
        public async Task<IActionResult> editExtension([FromBody] CreateExtensionApplicationModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExtensionApplicationModel extension = new ExtensionApplicationModel();

            // some code

            return Ok(extension);
        }

        [HttpDelete("extensionApplication/{id}")]
        public async Task<IActionResult> deleteExtension(Guid id)
        {

            // some code

            return Ok();
        }

        [HttpPost("extensionApplication/{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FullApplicationModel))]
        public async Task<IActionResult> EditStatusExtensionApplication([FromBody] ChangeStatusApplication status)
        {
            FullApplicationModel application = new FullApplicationModel();

            // some code

            return Ok(application);
        }
    }
}
