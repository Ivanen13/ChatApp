using Chat.Client.Services;
using Chat.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ChatApiClient _api;
        private readonly ChatHubClient _hub;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainViewModel(LoginViewModel login, ChatHubClient hub, ChatApiClient api)
        {
            _api = api;
            _hub = hub;
            _currentViewModel = login;
            login.LoggedIn += OnLoggedIn;
        }

        private async void OnLoggedIn(UserDto user)
        {
            var chat = new ChatViewModel(user, _api, _hub);
            CurrentViewModel = chat;
            await chat.InitializeAsync();
        }
    }
}
