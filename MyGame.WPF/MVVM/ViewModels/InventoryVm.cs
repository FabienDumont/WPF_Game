using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;

namespace MyGame.WPF.MVVM.ViewModels;

public class InventoryVm : BaseVm {
    public ICommand ReturnCommand { get; set; }

    public InventoryVm(INavigationService closeNavigationService) {
        ReturnCommand = new NavigateCommand(closeNavigationService);
    }
}