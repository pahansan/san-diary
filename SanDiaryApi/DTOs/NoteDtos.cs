using SanDiaryApi.Models;

namespace SanDiaryApi.DTOs
{
    public class CreateNoteRequest(string title, string content, Mood mood)
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public Mood Mood { get; set; } = mood;
    }

    public class GetNoteRequest(int id)
    {
        public int Id { get; set; } = id;
    }

    public class GetNotesByUserIdRequest()
    {
        // empty
    }

    public class UpdateNoteRequest(string title, string content, Mood mood)
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public Mood Mood { get; set; } = mood;
    }

    public class DeleteNoteRequest(int id)
    {
        public int Id { get; set; } = id;
    }

}
