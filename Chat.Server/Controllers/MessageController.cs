using Chat.Server.Data;
using Chat.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController(ChatDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> GetMessages([FromQuery] int take = 50)
        {
            take = Math.Clamp(take, 1, 100);

            var messages = await db.Messages
                .OrderByDescending(m => m.SentAt)
                .Take(take)
                .Select(m => new MessageDto(
                    m.Id,
                    m.User.Username,
                    m.Content,
                    DateTime.SpecifyKind(m.SentAt, DateTimeKind.Utc)))
                .ToListAsync();

            messages.Reverse();
            return messages;
        }
    }
}
