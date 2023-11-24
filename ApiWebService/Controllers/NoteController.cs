using ApiWebService.Contracts;
using ApiWebService.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using ApiWebService.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Azure;

namespace ApiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IETagService _etagService;

        public NoteController(INoteService noteService, IETagService etagService)
        {
            _noteService = noteService;
            _etagService = etagService;
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetNote(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
                return Results.NotFound();

            var result = new NoteResultModel
            {
                Id = note.Id,
                Content = note.Content,
                Title = note.Title,
                PersonId = note.PersonId
            };

            var etag = _etagService.GenerateETag(result);

            if (HttpContext.Request.Headers.IfNoneMatch == etag)
                return Results.StatusCode(304);

            return Results.Ok(new { 
                Result = result,
                ETag = etag
            });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IResult> GetNotesForUser(Guid id, [FromQuery] int pageNumber = 1)
        {
            var notes = await _noteService.GetNotesForUserAsync(new GetNotesRequestModel
            {
                PersonId = id,
                PageNumber = pageNumber,
            });

            var result = notes.Select(n => new NoteResultModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PersonId = n.PersonId,
            });

            var etag = _etagService.GenerateETag(result);

            if (HttpContext.Request.Headers.IfNoneMatch == etag)
                return Results.StatusCode(304);

            return Results.Ok(new {
                Result = result,
                ETag = etag
            });
        }

        [HttpGet]
        public async Task<IResult> GetAllNotes([FromQuery] int pageNumber = 1)
        {
            var notes = await _noteService.GetAllNotesAsync(new GetNotesRequestModel
            {
                PageNumber = pageNumber,
                PageSize = 20
            });

            var result = notes.Select(n => new NoteResultModel
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PersonId = n.PersonId,
            });

            var etag = _etagService.GenerateETag(result);

            if (HttpContext.Request.Headers.IfNoneMatch == etag)
                return Results.StatusCode(304);

            return Results.Ok(new
            {
                Result = result,
                ETag = etag
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveNote([FromBody] NoteSaveModel note)
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