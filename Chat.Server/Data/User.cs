namespace Chat.Server.Data
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
