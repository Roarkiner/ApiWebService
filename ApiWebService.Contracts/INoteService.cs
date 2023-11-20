using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;

namespace ApiWebService.Contracts
{
    public interface INoteService
    {
        Task<Note?> GetNoteByIdAsync(Guid noteId);
        Task<List<Note>> GetNotesForUserAsync(GetNotesRequestModel requestModel);
        Task<Note> SaveNoteAsync(NoteModel newNote);
        Task UpdateNoteAsync(NoteModel updateNote);
        Task DeleteNoteAsync(Guid noteId);
    }
}