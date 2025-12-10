using SanDiaryApi.DTOs;
using SanDiaryApi.Models;
using SanDiaryApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SanDiaryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/notes")]
    public class NotesController(NoteService noteService) : ControllerBase
    {
        private readonly NoteService _noteService = noteService;
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> Create([FromBody] CreateNoteRequest req)
        {
            var userId = GetCurrentUserId();

            var result = await _noteService.CreateNoteAsync(req, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            var userId = GetCurrentUserId();

            var result = await _noteService.GetNoteAsync(new GetNoteRequest(id), userId);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> Update(int id, [FromBody] UpdateNoteRequest req)
        {
            var userId = GetCurrentUserId();

            var result = await _noteService.UpdateNoteAsync(req, id, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();

            var result = await _noteService.DeleteNoteAsync(new DeleteNoteRequest(id), userId);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
