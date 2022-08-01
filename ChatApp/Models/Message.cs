using ChatApp.Models.Enum;

namespace ChatApp.Models
{
    public class Message 
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FromUserId { get; set; }
        public int InGroupId { get; set; }
        public FileType? FileType { get; set; }
    }
}
