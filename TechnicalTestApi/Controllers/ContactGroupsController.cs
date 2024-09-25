using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Application.Interfaces.ContactGroups;
using TechnicalTest.Shared.Models.ContactGroups;
using TechnicalTest.Shared.Wrapper;

namespace TechnicalTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactGroupsController : ControllerBase
    {
        #region Private Members & Constructor
        private readonly ILogger _logger;
        private readonly IContactGroupRepository _contactGroupRepository;

        public ContactGroupsController(ILogger<ContactGroupsController> logger, IContactGroupRepository contactGroupRepository)
        {
            _logger = logger;
            _logger.LogInformation("Contact Groups controller called");
            _contactGroupRepository = contactGroupRepository;
        }
        #endregion

        #region Public Functions

        [HttpPost("AddContactGroup")]
        public async Task<IActionResult> AddContactGroup([FromBody] ContactGroupAddRequest request)
        {
            Result<bool> result = await _contactGroupRepository.AddContactGroup(request);
            return Ok(result);
        }

        [HttpPut("EditContactGroup")]
        public async Task<IActionResult> EditContactGroup([FromBody] ContactGroupEditRequest request)
        {
            Result<bool> result = await _contactGroupRepository.EditContactGroup(request);
            return Ok(result);
        }

        [HttpGet("GetAllContactGroups")]
        public async Task<IActionResult> GetAllContactGroups()
        {
            _logger.LogInformation("Get All Contact Groups method called");
            var result = await _contactGroupRepository.GetAllContactGroups();

            return Ok(result);
        }

        #endregion
    }
}
