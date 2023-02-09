using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.MVVM.ViewModels; 

public class InformationVm : BaseVm {
    private readonly StringStore _stringStore;

    public string Message {
        get {
            return _stringStore.CurrentString ?? "Unknown";
        }
    }
    
    public ICommand ReturnCommand { get; set; }
    
    public InformationVm(StringStore stringStore, INavigationService closeNavigationService) {
        _stringStore = stringStore;
        
        ReturnCommand = new NavigateCommand(closeNavigationService);
    }
}