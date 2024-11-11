namespace gp_backend.Api.Dtos.Chat
{
    public class SendFileDto
    {
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
