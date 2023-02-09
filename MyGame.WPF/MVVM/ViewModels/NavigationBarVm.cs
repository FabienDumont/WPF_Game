using System.Windows;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Services;

namespace MyGame.WPF.MVVM.ViewModels;

public class NavigationBarVm : BaseVm {
    public ICommand GameNavigateCommand { get; set; }

    public NavigationBarVm(INavigationService gameNavigationService) {
        GameNavigateCommand = new NavigateCommand(gameNavigationService);
    }
}