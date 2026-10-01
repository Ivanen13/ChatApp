using Chat.Shared;
using Chat.Shared.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Client.Services
{
    public class ChatHubClient : IAsyncDisposable
    {
        private readonly HubConnection _connection;

        public event Action<MessageDto>? MessageReceived;
        public event Action<string>? StatusChanged;

        public ChatHubClient(string hubUrl)
        {
            _connection = new HubConnectionBuilder().WithUrl(hubUrl).WithAutomaticReconnect().Build();
            _connection.On<MessageDto>(nameof(IChatClient.ReceiveMessage), m => MessageReceived?.Invoke(m));

            _connection.Reconnecting += OnReconnecting;
            _connection.Reconnected += OnReconnected;
            _connection.Closed += OnClosed;
        }

        public async Task StartAsync()
        {
            const int maxAttempts = 5;

            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    StatusChanged?.Invoke("Pripájam sa...");
                    await _connection.StartAsync();
                    StatusChanged?.Invoke("Pripojené");
                    return;
                }
                catch when (attempt < maxAttempts)
                {
                    await Task.Delay(2000);
                }
            }
        }

        public Task SendMessageAsync(int userId, string content)
        {
            return _connection.InvokeAsync(nameof(IChatHub.SendMessage), userId, content);
        }

        public ValueTask DisposeAsync()
        {
            return _connection.DisposeAsync();
        }

        private Task OnReconnecting(Exception? error)
        {
            StatusChanged?.Invoke("Pripájam sa...");
            return Task.CompletedTask;
        }

        private Task OnReconnected(string? connectionId)
        {
            StatusChanged?.Invoke("Pripojené");
            return Task.CompletedTask;
        }

        private Task OnClosed(Exception? error)
        {
            StatusChanged?.Invoke("Odpojené");
            return Task.CompletedTask;
        }
    }
}
