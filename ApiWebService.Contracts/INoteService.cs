using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;

namespace ApiWebService.Contracts
{
    public interface INoteService
    {
        Task<Note?> GetNoteByIdAsync(Guid noteId);
        Task<List<Note>> GetNotesForUserAsync(GetNotesRequestModel requestModel);
        Task<List<Note>> GetAllNotesAsync(GetNotesRequestModel requestModel);
        Task<Note> SaveNoteAsync(NoteSaveModel newNote);
        Task UpdateNoteAsync(NoteUpdateModel updateNote, Guid noteId);
        Task DeleteNoteAsync(Guid noteId);
    }
}