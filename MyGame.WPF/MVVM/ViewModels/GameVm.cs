using System.Windows;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Services;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.MVVM.ViewModels;

public class GameVm : BaseVm {
    public ICommand CharacterNavigateCommand { get; set; }
    public ICommand InventoryNavigateCommand { get; set; }
    public ICommand SaveGameCommand { get; set; }
    public ICommand LoadGameCommand { get; set; }
    public ICommand MainMenuNavigateCommand { get; set; }

    public GameVm(SaveStore saveStore, CharacterStore characterStore, INavigationService characterNavigationService,
        INavigationService inventoryNavigationService,
        INavigationService mainMenuNavigationService) {
        CharacterNavigateCommand = new RelayCommand(parameters => {
            characterStore.CurrentCharacter = saveStore.CurrentSave!.Player;
            characterNavigationService.Navigate();
        });

        InventoryNavigateCommand = new NavigateCommand(inventoryNavigationService);
        SaveGameCommand = new SaveCommand(saveStore);
        LoadGameCommand = new LoadCommand(saveStore, null);
        MainMenuNavigateCommand = new NavigateCommand(mainMenuNavigationService);
    }
}