using Chat.Server.Data;
using Chat.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Chat.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(ChatDbContext db) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Username))
                return BadRequest("Username cant be empty.");

            string username = request.Username.Trim();

            if (username.Length > 32)
                return BadRequest("Username can be at most 32 characters.");

            User? user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                user = new User()
                { 
                    Username = username,
                    CreatedAt = DateTime.UtcNow,               
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();
            }

            return new UserDto(user.Id, user.Username);
        }
    }
}
