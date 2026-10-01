using Chat.Server.Data;
using Chat.Shared;
using Chat.Shared.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Server.Hubs
{
    public class ChatHub(ChatDbContext db) : Hub<IChatClient>, IChatHub
    {
        public async Task SendMessage(int userId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new HubException("Content cannot be empty");

            string contentTrim = content.Trim();

            if (contentTrim.Length > 1000)
                throw new HubException("Message can be at most 1000 characters.");

            User? user = await db.Users.FindAsync(userId);

            if (user == null)
                throw new HubException("User not found");

            Message message = new Message() 
            {
                Content = contentTrim,
                SentAt = DateTime.UtcNow,
                User = user,
                UserId = userId
            };

            db.Messages.Add(message);
            await db.SaveChangesAsync();

            MessageDto dto = new(message.Id, user.Username, contentTrim, message.SentAt);

            await Clients.All.ReceiveMessage(dto);
        }
    }
}
