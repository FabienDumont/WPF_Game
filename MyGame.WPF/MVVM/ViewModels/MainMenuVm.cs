using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.MVVM.ViewModels;

public class MainMenuVm : BaseVm {
    public ICommand CreateCharacterNavigateCommand { get; set; }
    public ICommand LoadGameCommand { get; set; }

    public MainMenuVm(
        SaveStore saveStore, StringStore stringStore, INavigationService createCharacterNavigationService, INavigationService gameNavigationService,
        INavigationService informationNavigationService
    ) {
        CreateCharacterNavigateCommand = new NavigateCommand(createCharacterNavigationService);
        LoadGameCommand = new LoadGameCommand(saveStore, stringStore, gameNavigationService, informationNavigationService);
    }
}