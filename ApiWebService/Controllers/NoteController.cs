using ApiWebService.Contracts;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using ApiWebService.Api.Extensions;

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
        public async Task<ActionResult<NoteResultModel>> GetNote(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
                return NotFound();

            return Ok(new NoteResultModel
            {
                Id = note.Id,
                Content = note.Content,
                Title = note.Title,
                PersonId = note.PersonId
            });
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<NoteResultModel>> GetNotesForUser(Guid id, [FromQuery] int pageNumber = 1)
        {
            var notes = await _noteService.GetNotesForUserAsync(new GetNotesRequestModel
            {
                PersonId = id,
                PageNumber = pageNumber,
            });

            return notes.Select(n => new NoteResultModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PersonId = n.PersonId,
            });
        }

        [HttpGet]
        public async Task<IEnumerable<NoteResultModel>> GetAllNotes([FromQuery] int pageNumber = 1)
        {
            var notes = await _noteService.GetAllNotesAsync(new GetNotesRequestModel
            {
                PageNumber = pageNumber,
                PageSize = 20
            });

            return notes.Select(n => new NoteResultModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PersonId = n.PersonId,
            });
        }

        [HttpPost]
        public async Task<ActionResult<NoteResultModel>> SaveNote([FromBody] NoteSaveModel note)
        {
            try
            {
                var createdNote = await _noteService.SaveNoteAsync(note);
                return this.Created(HttpContext, nameof(GetNote), createdNote, createdNote.Id.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteUpdateModel note)
        {
            try
            {
                await _noteService.UpdateNoteAsync(note, id);
                return NoContent();
            } catch (ArgumentException)
            {
                return NotFound(id);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}