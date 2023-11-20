using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;

namespace ApiWebService.Services
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _dbContext;

        public PersonService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void DeletePerson(Guid personId)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonById(Guid personId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetPersons()
        {
            throw new NotImplementedException();
        }

        public void SavePerson(Person newPerson)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person updatePerson)
        {
            throw new NotImplementedException();
        }
    }
}