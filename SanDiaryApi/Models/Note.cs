namespace SanDiaryApi.Models
{
    public class Note(string title, string content, Mood mood, int userId)
    {
        public int Id { get; set; }
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
        public Mood Mood { get; set; } = mood;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int UserId { get; set; } = userId;
    }
}
