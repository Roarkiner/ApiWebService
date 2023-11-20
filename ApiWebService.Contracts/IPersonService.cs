using ApiWebService.Models.DataModels;

namespace ApiWebService.Contracts
{
    public interface IPersonService
    {
        Person GetPersonById(Guid personId);
        IEnumerable<Person> GetPersons();
        void SavePerson(Person newPerson);
        void UpdatePerson(Person updatePerson);
        void DeletePerson(Guid personId);
    }
}