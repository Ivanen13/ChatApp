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
                throw new HubException("Správa nesmie byť prázdna");

            string contentTrim = content.Trim();

            if (contentTrim.Length > 1000)
                throw new HubException("Správa môže mať maximálne 1000 znakov");

            User? user = await db.Users.FindAsync(userId);

            if (user == null)
                throw new HubException("Používateľ sa nenašiel");

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
