using Chat.Client.Services;
using Chat.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Windows;

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
            ErrorMessage = string.Empty;
            try
            {
                var user = await api.LoginAsync(Username);
                LoggedIn?.Invoke(user);
            }
            catch (InvalidOperationException ex)
            {
                // validacna chyba zo servera
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex) when (ex.StatusCode is null)
            {
                // server nebezi / nie je dostupny
                ErrorMessage = "Server nie je dostupný";
            }
            catch (HttpRequestException)
            {
                // server odpovedal, ale s chybou (napr. 500)
                ErrorMessage = "Nastala chyba na serveri.";
            }           
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
