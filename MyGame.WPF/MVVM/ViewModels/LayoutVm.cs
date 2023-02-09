using MVVMEssentials.ViewModels;

namespace MyGame.WPF.MVVM.ViewModels;

public class LayoutVm : BaseVm {
    public NavigationBarVm NavigationBarVm { get; }
    public BaseVm ContentVm { get; }

    public LayoutVm(NavigationBarVm navigationBarVm, BaseVm contentVm) {
        NavigationBarVm = navigationBarVm;
        ContentVm = contentVm;
    }

    public override void Dispose() {
        NavigationBarVm.Dispose();
        ContentVm.Dispose();

        base.Dispose();
    }
}