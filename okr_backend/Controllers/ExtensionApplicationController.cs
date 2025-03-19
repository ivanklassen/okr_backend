
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using okr_backend.Models;
using okr_backend.Persistence;

namespace okr_backend.Controllers
{
    [ApiController]
    [Authorize]

    public class ExtensionApplicationController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ExtensionApplicationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [Authorize]
        [HttpPost("extensionApplication/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExtensionApplicationModel))]
        public async Task<IActionResult> createExtension([FromBody] CreateExtensionApplicationModel model, Guid applicationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ExtensionApplicationModel extension = new ExtensionApplicationModel();

            extensionApplication ext = new extensionApplication();

            ext.Id = Guid.NewGuid();
            ext.extensionToDate = model.extensionToDate;
            ext.applicationId = applicationId;
            ext.description = model.description;
            ext.image = model.image;
            ext.status = Status.inProcess;

            await _context.AddAsync(ext);
            await _context.SaveChangesAsync();

            extension.Id = ext.Id;
            extension.extensionToDate = model.extensionToDate;
            extension.applicationId = applicationId;
            extension.description = model.description;
            extension.image = model.image;
            extension.status = Status.inProcess;

            return Ok(extension);
        }

        [Authorize]
        [HttpPut("extensionApplication/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExtensionApplicationModel))]
        public async Task<IActionResult> editExtension([FromBody] CreateExtensionApplicationModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_context.extensionApplications.FirstOrDefault(p => p.Id == id) == null) return NotFound();

            var ext = await _context.extensionApplications.FirstOrDefaultAsync(p => p.Id == id);

            ext.extensionToDate = model.extensionToDate;
            ext.description = model.description;
            ext.image = model.image;

            await _context.SaveChangesAsync();

            ExtensionApplicationModel extension = new ExtensionApplicationModel();

            extension.Id = ext.Id;
            extension.applicationId = ext.applicationId;
            extension.extensionToDate = model.extensionToDate;
            extension.description = model.description;
            extension.image = model.image;
            extension.status = ext.status;

            return Ok(extension);
        }

        [Authorize]
        [HttpDelete("extensionApplication/{id}")]
        public async Task<IActionResult> deleteExtension(Guid id)
        {
            if (_context.extensionApplications.FirstOrDefault(p => p.Id == id) == null) return NotFound();

            await _context.extensionApplications.Where(p => p.Id == id).ExecuteDeleteAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("extensionApplication/{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FullApplicationModel))]
        public async Task<IActionResult> EditStatusExtensionApplication([FromBody] ChangeStatusApplication status, Guid id)
        {
            if (_context.extensionApplications.FirstOrDefault(p => p.Id == id) == null) return NotFound();

            var ext = await _context.extensionApplications.FirstOrDefaultAsync(p => p.Id == id);

            if (status.status == Status.Accepted)
            {
                ext.status = Status.Accepted;
            }
            if (status.status == Status.Rejected)
            {
                ext.status = Status.Rejected;
            }

            ext.comment = status.comment;

            await _context.SaveChangesAsync();

            FullApplicationModel model = _context.Applications.Where(p => p.Id == ext.applicationId).Include(p => p.extensions)
                .Select(p => new FullApplicationModel
                {
                    Id = p.Id,
                    userId = p.userId,
                    fromDate = p.fromDate,
                    toDate = p.toDate,
                    description = p.description,
                    image = p.image,
                    status = p.status,
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
