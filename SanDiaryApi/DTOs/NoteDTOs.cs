using SanDiaryApi.Models;

namespace SanDiaryApi.DTOs
{
    public class CreateNoteRequest(string title, string content, Mood mood, int userId)
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public Mood Mood { get; set; } = mood;
        public int UserId { get; set; } = userId;
    }

    public class GetNoteRequest(int id, int userId)
    {
        public int Id { get; set; } = id;
        public int UserId { get; set; } = userId;
    }

    public class GetNotesByUserIdRequest(int userId)
    {
        public int UserId { get; set; } = userId;
    }

    public class UpdateNoteRequest(int id, string title, string content, Mood mood, int userId)
    {
        public int Id { get; set; } = id;
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public Mood Mood { get; set; } = mood;
        public int UserId { get; set; } = userId;
    }

    public class DeleteNoteRequest(int id, int userId)
    {
        public int Id { get; set; } = id;
        public int UserId { get; set; } = userId;
    }

}
