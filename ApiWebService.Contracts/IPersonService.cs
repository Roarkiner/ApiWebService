using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;

namespace ApiWebService.Contracts
{
    public interface IPersonService
    {
        Task<Person?> GetPersonByIdAsync(Guid noteId);
        Task<List<Person>> GetAllPersonsAsync(GetPersonsRequestModel requestModel);
        Task<Person> SavePersonAsync(PersonSaveModel newPerson);
        Task UpdatePersonAsync(PersonUpdateModel updatePerson, Guid personId);
        Task DeletePersonAsync(Guid noteId);
        Task<bool> Login(string email, string password);
    }
}