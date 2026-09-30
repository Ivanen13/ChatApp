namespace Chat.Server.Data
{
    public class Message
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
