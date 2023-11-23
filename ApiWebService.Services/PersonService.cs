using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ApiWebService.Services
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _dbContext;

        public PersonService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person?> GetPersonByIdAsync(Guid personId)
        {
            return await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId && !p.IsDeleted);
        }

        public async Task<List<Person>> GetAllPersonsAsync(GetPersonsRequestModel requestModel)
        {
            return await Task.FromResult(_dbContext.Persons
                .Where(p => !p.IsDeleted)
                .Skip(requestModel!.PageSize * (requestModel.PageNumber - 1))
                .Take(requestModel.PageSize)
                .ToList());
        }

        public async Task<Person> SavePersonAsync(PersonSaveModel newPerson)
        {
            var errors = ValidateSavePerson(newPerson);
            if (errors.Any())
            {
                throw new Exception(string.Join(". ", errors) + ".");
            }

            var personToSave = new Person
            {
                Email = newPerson.Email,
                Password = newPerson.Password
            };

            await _dbContext.AddAsync(personToSave);
            _dbContext.SaveChanges();
            return personToSave;
        }

        public async Task UpdatePersonAsync(PersonUpdateModel updatePerson, Guid personId)
        {
            if (string.IsNullOrWhiteSpace(updatePerson.Email))
            {
                throw new Exception("The Email can't be empty");
            }

            var modelToUpdate = await GetPersonByIdAsync(personId);

            if (modelToUpdate == null)
            {
                throw new ArgumentException($"The person with {personId} doesn't exist");
            }

            modelToUpdate.Email = updatePerson.Email;
            _dbContext.SaveChanges();
        }

        public async Task DeletePersonAsync(Guid personId)
        {
            var modelToDelete = await GetPersonByIdAsync(personId);

            if (modelToDelete == null)
            {
                throw new ArgumentException($"The person with {personId} doesn't exist");
            }

            modelToDelete.IsDeleted = true;
            var notes = _dbContext.Notes
                .Where(n => n.PersonId == personId);

            foreach (var note in notes) 
            {
                note.IsDeleted = true;
            }

            _dbContext.SaveChanges();
        }

        public Task<bool> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        private static List<string> ValidateSavePerson(PersonSaveModel personModel)
        {
            var errorList = new List<string>();
            
            if (string.IsNullOrWhiteSpace(personModel.Email))
            {
                errorList.Add("The Email can't be empty");
            }
            if (string.IsNullOrWhiteSpace(personModel.Password))
            {
                errorList.Add("The password can't be empty");
            } 
            else if (!Regex.IsMatch(personModel.Password, @"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$"))
            {
                errorList.Add("Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)");
            }

            return errorList;
        }
    }
}
