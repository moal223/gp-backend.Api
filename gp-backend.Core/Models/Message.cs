namespace gp_backend.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string? Content { get; set; }
        public string Type { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public string? FilePath { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
