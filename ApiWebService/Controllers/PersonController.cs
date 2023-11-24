using ApiWebService.Api.Extensions;
using ApiWebService.Contracts;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IETagService _etagService;

        public PersonController(IPersonService personService, IETagService etagService)
        {
            _personService = personService;
            _etagService = etagService;
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetPerson(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            
            if (person == null)
                return Results.NotFound();

            var result = new PersonResultModel
            {
                Id = person.Id,
                Email = person.Email
            };

            var etag = _etagService.GenerateETag(result);

            if (HttpContext.Request.Headers.IfNoneMatch == etag)
                return Results.StatusCode(304);

            return Results.Ok(new
            {
                Result = result,
                ETag = etag
            });
        }

        [HttpGet]
        public async Task<IResult> GetAllPersons([FromQuery] int pageNumber = 1)
        {
            var persons = await _personService.GetAllPersonsAsync(new GetPersonsRequestModel
            {
                PageNumber = pageNumber,
                PageSize = 20
            });

            var result = persons.Select(n => new PersonResultModel
            {
                Id = n.Id,
                Email = n.Email
            });

            var etag = _etagService.GenerateETag(result);

            if (HttpContext.Request.Headers.IfNoneMatch == etag)
                return Results.StatusCode(304);

            return Results.Ok(new
            {
                Result = result,
                ETag = etag
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