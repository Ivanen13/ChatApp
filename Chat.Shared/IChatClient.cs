using Chat.Shared.Dtos;

namespace Chat.Shared
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessageDto message);
    }
}
