using Chat.Client.Services;
using Chat.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;


namespace Chat.Client.ViewModels
{
    public partial class ChatViewModel(UserDto user, ChatApiClient api, ChatHubClient hub) : ObservableObject
    {
        public string Username => user.Username;

        public ObservableCollection<MessageItem> Messages { get; } = [];

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendCommand))]
        private string _newMessage = "";

        [ObservableProperty]
        private string _status = "Pripájam sa...";

        [ObservableProperty]
        private string? _errorMessage;

        public async Task InitializeAsync()
        {

            hub.MessageReceived += m =>
                Application.Current?.Dispatcher.InvokeAsync(() =>
                    Messages.Add(new MessageItem(m, m.Username == user.Username)));

            hub.StatusChanged += s =>
                Application.Current?.Dispatcher.InvokeAsync(() => Status = s);

            try
            {
                await hub.StartAsync();

                var history = await api.GetHistoryAsync();
                var existingIds = Messages.Select(m => m.Id).ToHashSet();

                int index = 0;
                foreach (var m in history.Where(m => !existingIds.Contains(m.Id)))
                    Messages.Insert(index++, new MessageItem(m, m.Username == user.Username));
            }
            catch (Exception)
            {
                Status = "Server nie je dostupný";
            }
        }

        private bool CanSend()
        {
            return !string.IsNullOrWhiteSpace(NewMessage);
        }

        [RelayCommand(CanExecute = nameof(CanSend))]
        private async Task SendAsync()
        {
            ErrorMessage = string.Empty;

            try
            {
                await hub.SendMessageAsync(user.Id, NewMessage);
                NewMessage = string.Empty;
            }
            catch (Exception)
            {
                ErrorMessage = "Správu sa nepodarilo odoslať.";
            }
        }
    }
}
    
