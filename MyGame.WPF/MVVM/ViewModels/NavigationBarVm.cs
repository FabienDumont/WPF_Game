using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;

namespace MyGame.WPF.MVVM.ViewModels;

public class NavigationBarVm : BaseVm {
    public ICommand GameNavigateCommand { get; set; }

    public NavigationBarVm(INavigationService gameNavigationService) {
        GameNavigateCommand = new NavigateCommand(gameNavigationService);
    }
}