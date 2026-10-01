using Chat.Client.Services;
using Chat.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;

namespace Chat.Client.ViewModels
{
    public partial class LoginViewModel(ChatApiClient api) : ObservableObject
    {
        public event Action<UserDto>? LoggedIn;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _username = "";

        [ObservableProperty]
        private string? _errorMessage;

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            ErrorMessage = null;
            try
            {
                var user = await api.LoginAsync(Username);
                LoggedIn?.Invoke(user);
            }
            catch (InvalidOperationException ex) 
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = ex.Message;
            }           
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
