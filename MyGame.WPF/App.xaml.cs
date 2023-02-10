using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Services;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.ViewModels;
using MyGame.WPF.MVVM.Views;

namespace MyGame.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
    private readonly IHost _host;
    private readonly ServiceProvider _serviceProvider;

    public App() {
        _host = Host.CreateDefaultBuilder().ConfigureAppConfiguration(
            c => {
                c.AddJsonFile("appsettings.json");
                c.AddEnvironmentVariables();
            }
        ).ConfigureServices(
            (context, services) => {
                // Store classes to send information through ViewModels
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();

                services.AddSingleton<StringStore>();
                services.AddSingleton<CharacterStore>();
                services.AddSingleton<SaveStore>();

                services.AddSingleton<CloseModalNavigationService>();

                // Services creation to allow ViewModels to navigate from one to another

                services.AddTransient<MainMenuVm>(
                    s => new MainMenuVm(
                        s.GetRequiredService<SaveStore>(), s.GetRequiredService<StringStore>(), s.GetRequiredService<NavigationService<CreateCharacterVm>>(),
                        s.GetRequiredService<NavigationService<GameVm>>(), CreateInformationNavigationService(s)
                    )
                );

                services.AddSingleton(s => new NavigationService<MainMenuVm>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<MainMenuVm>));

                services.AddTransient<CreateCharacterVm>(
                    s => new CreateCharacterVm(s.GetRequiredService<SaveStore>(), s.GetRequiredService<NavigationService<GameVm>>())
                );

                services.AddSingleton(
                    s => new NavigationService<CreateCharacterVm>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<CreateCharacterVm>)
                );

                services.AddTransient(
                    s => new GameVm(
                        s.GetRequiredService<SaveStore>(), s.GetRequiredService<StringStore>(), s.GetRequiredService<CharacterStore>(),
                        CreateCharacterNavigationService(s), CreateInventoryNavigationService(s), s.GetRequiredService<NavigationService<MainMenuVm>>(),
                        CreateInformationNavigationService(s)
                    )
                );

                services.AddSingleton(s => new NavigationService<GameVm>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<GameVm>));

                services.AddTransient(s => new CharacterVm(s.GetRequiredService<CharacterStore>(), s.GetRequiredService<CloseModalNavigationService>()));
                services.AddTransient(s => new InventoryVm(s.GetRequiredService<CloseModalNavigationService>()));
                services.AddTransient(s => new InformationVm(s.GetRequiredService<StringStore>(), s.GetRequiredService<CloseModalNavigationService>()));
                

                // Nav Bar
                services.AddTransient(s => new NavigationBarVm(CreateGameNavigationService(s)));

                // Creation of the Main Window which can display the User Controls
                services.AddSingleton<MainVm>();
                services.AddSingleton(s => new MainWindow() { DataContext = s.GetRequiredService<MainVm>() });
            }
        ).Build();
    }

    protected override void OnStartup(StartupEventArgs e) {
        _host.Start();

        INavigationService mainMenuNavigationService = _host.Services.GetRequiredService<NavigationService<MainMenuVm>>();
        mainMenuNavigationService.Navigate();

        // Showing the main Window
        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await _host.StopAsync();
        _host.Dispose();
    }

    private static INavigationService CreateGameNavigationService(IServiceProvider serviceProvider) {
        // A layout is applied 
        return new LayoutNavigationService<GameVm>(
            serviceProvider.GetRequiredService<NavigationStore>(), serviceProvider.GetRequiredService<GameVm>,
            serviceProvider.GetRequiredService<NavigationBarVm>
        );
    }

    private static INavigationService CreateCharacterNavigationService(IServiceProvider serviceProvider) {
        return new ModalNavigationService<CharacterVm>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(), serviceProvider.GetRequiredService<CharacterVm>
        );
    }

    private static INavigationService CreateInventoryNavigationService(IServiceProvider serviceProvider) {
        return new ModalNavigationService<InventoryVm>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(), serviceProvider.GetRequiredService<InventoryVm>
        );
    }

    private static INavigationService CreateInformationNavigationService(IServiceProvider serviceProvider) {
        return new ModalNavigationService<InformationVm>(
            serviceProvider.GetRequiredService<ModalNavigationStore>(), serviceProvider.GetRequiredService<InformationVm>
        );
    }
}