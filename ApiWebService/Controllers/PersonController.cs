using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
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
        public Person GetPerson(Guid id)
        {
            return _personService.GetPersonById(id);
        }

        [HttpGet]
        public IEnumerable<Person> GetAllPersons()
        {
            return _personService.GetPersons();
        }

        [HttpPost]
        public void SavePerson(Person person) 
        {
            _personService.SavePerson(person);
        }

        [HttpPut("{id}")]
        public void UpdatePerson(Person person) 
        {
            _personService.UpdatePerson(person);
        }

        [HttpDelete("{id}")]
        public void DeletePerson(Guid id) 
        {
            _personService.DeletePerson(id);
        }
    }
}
