using ApiWebService.Contracts;
using ApiWebService.Models.DataModels;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpGet]
        public async Task<IEnumerable<NoteModel>> GetNotesForUser(Guid personId, int pageNumber = 1)
        {
            var notes = await _noteService.GetNotesForUserAsync(new GetNotesRequestModel
            {
                PersonId = personId,
                PageNumber = pageNumber,
                PageSize = 10
            });

            return notes.Select(n => new NoteModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PersonId = n.PersonId,
            });
        }

        [HttpPost]
        public async Task<ActionResult<NoteModel>> SaveNote(NoteModel note)
        {
            try
            {
                var createdNote = await _noteService.SaveNoteAsync(note);
                return CreatedAtAction(nameof(GetNote), createdNote.Id, createdNote);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(NoteModel note)
        {
            try
            {
                await _noteService.UpdateNoteAsync(note);
                return Ok();
            } catch (ArgumentException)
            {
                return NotFound(note.Id);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public void DeleteNote(Guid id)
        {
            _noteService.DeleteNoteAsync(id);
        }
    }
}
