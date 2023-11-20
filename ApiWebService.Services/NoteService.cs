using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace ApiWebService.Services
{
    public class NoteService : INoteService
    {
        private readonly DataContext _dbContext;

        public NoteService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Note?> GetNoteByIdAsync(Guid noteId)
        {
            return await _dbContext.Notes
                .Include(n => n.Person)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Note>> GetNotesForUserAsync(GetNotesRequestModel requestModel)
        {
            var person = _dbContext.Persons
                .Include(p => p.Notes)
                .FirstOrDefault(p => p.Id == requestModel.PersonId);
            
            if(person == null)
            {
                throw new ArgumentException($"The person with {requestModel.PersonId} doesn't exist");
            }

            return person.Notes
                .Skip(requestModel!.PageSize * requestModel.PageNumber)
                .Take(requestModel.PageSize)
                .ToList();
        }

        public async Task<Note> SaveNoteAsync(NoteModel newNote)
        {
            var errors = ValidateNote(newNote, true);
            if(errors.Any())
            {
                throw new Exception(String.Join(". ", errors) + ".");
            }
            
            var noteToSave = new Note
            {
                Title = newNote.Title ?? "",
                Content = newNote.Content ?? "",
                PersonId = newNote.PersonId
            };
            
            await _dbContext.AddAsync(noteToSave);
            _dbContext.SaveChanges();
            return noteToSave;
        }

        public async Task UpdateNoteAsync(NoteModel updateNote)
        {
            var errors = ValidateNote(updateNote, false);
            if (errors.Any())
            {
                throw new Exception(String.Join(". ", errors) + ".");
            }

            var modelToUpdate = await GetNoteByIdAsync(updateNote.Id);

            if (modelToUpdate == null)
            {
                throw new ArgumentException($"The note with {updateNote.Id} doesn't exist");
            }

            modelToUpdate.Title = updateNote.Title ?? "";
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

            _dbContext.Remove(modelToDelete);
        }

        private static List<string> ValidateNote(NoteModel noteModel, bool isCreation)
        {
            var errorList = new List<string>();

            if (noteModel?.Title == null || noteModel?.Title == string.Empty)
            {
                errorList.Add("The title can't be empty");
            }
            if (isCreation && noteModel?.PersonId == Guid.Empty)
            {
                errorList.Add("The personId can't be empty");
            }

            return errorList;
        }
    }
}
