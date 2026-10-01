using Chat.Client.Services;
using Chat.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.Client.ViewModels
{
    public partial class ChatViewModel(UserDto user, ChatApiClient api, ChatHubClient hub) : ObservableObject
    {
        public string Username => user.Username;
    }
}
