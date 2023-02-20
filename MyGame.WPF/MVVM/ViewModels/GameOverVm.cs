using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.MVVM.ViewModels; 

public class GameOverVm : BaseVm {
    private readonly SaveStore _saveStore;

    public string Reason => _saveStore.CurrentSave!.GameOver;
    public ICommand MainMenuNavigateCommand { get; set; }

    public GameOverVm(SaveStore saveStore, INavigationService mainMenuNavigationService) {
        _saveStore = saveStore;
        MainMenuNavigateCommand = new NavigateCommand(mainMenuNavigationService);
    }
}