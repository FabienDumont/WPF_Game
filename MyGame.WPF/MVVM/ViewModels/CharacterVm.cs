using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.MVVM.ViewModels; 

public class CharacterVm : BaseVm {
    private readonly CharacterStore _characterStore;


    public string Name {
        get {
            return _characterStore.CurrentCharacter?.Name ?? "Unknown";
        }
    }
    
    public ICommand ReturnCommand { get; set; }
    
    public CharacterVm(CharacterStore characterStore, INavigationService closeNavigationService) {
        _characterStore = characterStore;
        ReturnCommand = new NavigateCommand(closeNavigationService);
    }
}