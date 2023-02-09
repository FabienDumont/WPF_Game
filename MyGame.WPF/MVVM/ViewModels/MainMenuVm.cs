using System.IO;
using System.Text.Json;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.MVVM.ViewModels; 

public class MainMenuVm : BaseVm {
    public ICommand CreateCharacterNavigateCommand { get; set; }
    public ICommand LoadGameCommand { get; set; }

    public MainMenuVm(SaveStore saveStore, INavigationService createCharacterNavigationService, INavigationService gameNavigationService) {
        CreateCharacterNavigateCommand = new NavigateCommand(createCharacterNavigationService);
        LoadGameCommand = new LoadCommand(saveStore, gameNavigationService);
    }
}