using Chat.Shared;
using Chat.Shared.Dtos;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Chat.Client.Services
{
    public class ChatApiClient(HttpClient http)
    {
        public async Task<UserDto> LoginAsync(string username)
        {
            var response =  await http.PostAsJsonAsync("api/users/login", new LoginRequest(username));

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new InvalidOperationException(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<UserDto>())!;
        }

        public async Task<List<MessageDto>> GetHistoryAsync(int take = 50)
        {
            return await http.GetFromJsonAsync<List<MessageDto>>($"api/messages?take={take}") ?? [];
        }
    }
}
