using ApiWebService.Api.Extensions;
using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonResultModel>> GetPerson(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);

            if (person == null)
                return NotFound();

            return Ok(new PersonResultModel
            {
                Id = person.Id,
                Email = person.Email
            });
        }

        [HttpGet]
        public async Task<IEnumerable<PersonResultModel>> GetAllPersons([FromQuery] int pageNumber = 1)
        {
            var persons = await _personService.GetAllPersonsAsync(new GetPersonsRequestModel
            {
                PageNumber = pageNumber,
                PageSize = 20
            });

            return persons.Select(n => new PersonResultModel
            {
                Id = n.Id,
                Email = n.Email
            });
        }

        [HttpPost]
        public async Task<ActionResult<PersonResultModel>> SavePerson([FromBody] PersonSaveModel person)
        {
            try
            {
                var createdPerson = await _personService.SavePersonAsync(person);
                return this.Created(HttpContext, nameof(GetPerson), createdPerson, createdPerson.Id.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] PersonUpdateModel person)
        {
            try
            {
                await _personService.UpdatePersonAsync(person, id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePersonAsync(id);
            return NoContent();
        }
    }
}