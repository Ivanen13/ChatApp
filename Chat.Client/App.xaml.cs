using Chat.Client.Services;
using Chat.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace Chat.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string ServerUrl = "http://localhost:5076/";
    private ServiceProvider? _services;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient { BaseAddress = new Uri(ServerUrl) });
        services.AddSingleton<ChatApiClient>();
        services.AddSingleton(_ => new ChatHubClient(ServerUrl + "hubs/chat"));

        services.AddSingleton<MainWindow>();

        services.AddSingleton<MainViewModel>();
        services.AddTransient<LoginViewModel>();

        _services = services.BuildServiceProvider();
        _services.GetRequiredService<MainWindow>().Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_services is not null)
            await _services.DisposeAsync();

        base.OnExit(e);
    }

}

