using SanDiaryApi.Common;
using SanDiaryApi.Models;
using SanDiaryApi.Data;
using SanDiaryApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace SanDiaryApi.Services
{
    public class NoteService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<Result<Note>> CreateNoteAsync(CreateNoteRequest req, int userId)
        {
            var errors = ValidateNote(req.Title, req.Content, req.Mood, userId);
            if (errors.Count != 0)
            {
                return Result<Note>.Fail(errors);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result<Note>.Fail("User does not exist.");
            }

            var now = DateTimeOffset.UtcNow;
            var note = new Note(req.Title, req.Content, req.Mood, userId)
            {
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Result<Note>.Success(note);
        }

        public async Task<Result<Note>> GetNoteAsync(GetNoteRequest req, int userId)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == req.Id && n.UserId == userId);
            if (note == null)
            {
                return Result<Note>.Fail("Note does not exist.");
            }
            return Result<Note>.Success(note);
        }

        public async Task<Result<List<Note>>> GetNotesByUserIdAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result<List<Note>>.Fail("User does not exist.");
            }
            var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
            return Result<List<Note>>.Success(notes);
        }

        public async Task<Result<Note>> UpdateNoteAsync(UpdateNoteRequest req, int id, int userId)
        {
            var errors = ValidateNote(req.Title, req.Content, req.Mood, userId);
            if (errors.Count != 0)
            {
                return Result<Note>.Fail(errors);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Result<Note>.Fail("User does not exist.");
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null)
            {
                return Result<Note>.Fail("Note does not exist.");
            }

            note.Title = req.Title;
            note.Content = req.Content;
            note.Mood = req.Mood;
            note.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
            return Result<Note>.Success(note);
        }

        public async Task<Result<bool>> DeleteNoteAsync(DeleteNoteRequest req, int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == req.Id && n.UserId == userId);
            if (note == null)
            {
                return Result<bool>.Fail("Note does not exist.");
            }
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        private static List<string> ValidateNote(string title, string content, Mood mood, int userId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(title))
                errors.Add("Title is required.");

            if (title?.Length > 200)
                errors.Add("Title cannot exceed 200 characters.");

            if (string.IsNullOrWhiteSpace(content))
                errors.Add("Content is required.");

            if (!Enum.IsDefined(mood))
                errors.Add("Invalid mood.");

            if (userId <= 0)
                errors.Add("Invalid user ID.");

            return errors;
        }
    }
}
