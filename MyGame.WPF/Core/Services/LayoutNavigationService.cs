using System;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.ViewModels;

namespace MyGame.WPF.Core.Services;

public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : BaseVm {
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createVm;
    private readonly Func<NavigationBarVm> _createNavigationBarVm;

    public LayoutNavigationService(
        NavigationStore navigationStore, Func<TViewModel> createVm, Func<NavigationBarVm> createNavigationBarVm
    ) {
        _navigationStore = navigationStore;
        _createVm = createVm;
        _createNavigationBarVm = createNavigationBarVm;
    }

    public void Navigate() {
        _navigationStore.CurrentViewModel = new LayoutVm(_createNavigationBarVm(), _createVm());
    }
}