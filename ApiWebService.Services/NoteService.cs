using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace ApiWebService.Services
{
    public class NoteService : INoteService
    {
        private readonly DataContext _dbContext;
        private readonly IPersonService _personService;

        public NoteService(DataContext dbContext, IPersonService personService)
        {
            _dbContext = dbContext;
            _personService = personService;
        }

        public async Task<Note?> GetNoteByIdAsync(Guid noteId)
        {
            return await _dbContext.Notes
                .FirstOrDefaultAsync(n => n.Id == noteId && !n.IsDeleted);
        }

        public async Task<List<Note>> GetNotesForUserAsync(GetNotesRequestModel requestModel)
        {
            var person = await _personService.GetPersonByIdAsync(requestModel.PersonId);
            
            if(person == null)
            {
                throw new ArgumentException($"The person with {requestModel.PersonId} doesn't exist");
            }

            return await _dbContext.Notes
                .Where(n => n.PersonId == person.Id && !n.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Note>> GetAllNotesAsync(GetNotesRequestModel requestModel)
        {
            return await Task.FromResult(_dbContext.Notes
                .Where(n => !n.IsDeleted)
                .Skip(requestModel!.PageSize * (requestModel.PageNumber - 1))
                .Take(requestModel.PageSize)
                .ToList());
        }

        public async Task<Note> SaveNoteAsync(NoteSaveModel newNote)
        {
            var errors = await ValidateSaveNoteAsync(newNote);
            if(errors.Any())
            {
                throw new Exception(string.Join(". ", errors) + ".");
            }
            
            var noteToSave = new Note
            {
                Title = newNote.Title,
                Content = newNote.Content ?? "",
                PersonId = newNote.PersonId
            };
            
            await _dbContext.AddAsync(noteToSave);
            _dbContext.SaveChanges();
            return noteToSave;
        }

        public async Task UpdateNoteAsync(NoteUpdateModel updateNote, Guid noteId)
        {
            var modelToUpdate = await GetNoteByIdAsync(noteId);

            if (modelToUpdate == null)
            {
                throw new ArgumentException($"The note with {noteId} doesn't exist");
            }

            if (!string.IsNullOrWhiteSpace(updateNote.Title))
            {
                modelToUpdate.Title = updateNote.Title;
            }
            modelToUpdate.Content = updateNote.Content ?? "";
            _dbContext.SaveChanges();
        }

        public async Task DeleteNoteAsync(Guid noteId)
        {
            var modelToDelete = await GetNoteByIdAsync(noteId);

            if (modelToDelete == null)
            {
                throw new ArgumentException($"The note with {noteId} doesn't exist");
            }

            modelToDelete.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        private async Task<List<string>> ValidateSaveNoteAsync(NoteSaveModel noteModel)
        {
            var errorList = new List<string>();

            var person = await _personService.GetPersonByIdAsync(noteModel.PersonId);

            if (person == null)
            {
                errorList.Add($"The person with {noteModel.PersonId} doesn't exist");
            }

            if (noteModel?.Title == null || noteModel?.Title == string.Empty)
            {
                errorList.Add("The title can't be empty");
            }
            if (noteModel?.PersonId == Guid.Empty)
            {
                errorList.Add("The personId can't be empty");
            }

            return errorList;
        }
    }
}
