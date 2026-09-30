namespace Chat.Shared.Dtos
{
    public record UserDto(int Id, string Username);
    public record MessageDto(int Id, string Username, string Content, DateTime SentAt);
    public record LoginRequest(string Username);

}
