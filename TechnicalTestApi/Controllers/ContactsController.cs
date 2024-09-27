using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Application.Interfaces.Contacts;
using TechnicalTest.Shared.Models.Contacts;
using TechnicalTest.Shared.Wrapper;

namespace TechnicalTest.Api.Controllers
{
    [ApiController]
    public class ContactsController : BaseController
    {
        #region Private Members & Constructor
        private readonly ILogger _logger;
        private readonly IContactRepository _contactRepository;

        public ContactsController(ILogger<ContactsController> logger, IContactRepository contactRepository)
        {
            _logger = logger;
            _logger.LogInformation("Contacts controller called");
            _contactRepository = contactRepository;
        }
        #endregion

        #region Public Functions

        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] ContactAddRequest request)
        {
            Result<bool> result = await _contactRepository.AddContact(request);
            return Ok(result);
        }

        [HttpPut("EditContact")]
        public async Task<IActionResult> EditContact([FromBody] ContactEditRequest request)
        {
            Result<bool> result = await _contactRepository.EditContact(request);
            return Ok(result);
        }

        [HttpDelete("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            _logger.LogInformation("Delete Contact method called");
            var result = await _contactRepository.DeleteContact(id);
            return Ok(result);
        }

        [HttpGet("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            _logger.LogInformation("Get All Contacts method called");
            var result = await _contactRepository.GetAllContacts();

            return Ok(result);
        }

        [HttpGet("GetContactById/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            _logger.LogInformation("Get Contact by Id method called");
            var result = await _contactRepository.GetContactById(id);
            return Ok(result);
        }

        #endregion
    }
}
