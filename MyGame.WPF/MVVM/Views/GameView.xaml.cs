using System;
using System.Windows.Controls;

namespace MyGame.WPF.MVVM.Views;

public partial class GameView {
    public GameView() {
        InitializeComponent();
    }
    
    private bool _autoScroll = true;

    private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e) {
        if (e.ExtentHeightChange == 0) {
            if (Math.Abs(((ScrollViewer)sender).VerticalOffset - ((ScrollViewer)sender).ScrollableHeight) < 0.01) {
                _autoScroll = true;
            }
            else {
                _autoScroll = false;
            }
        }

        if (_autoScroll && e.ExtentHeightChange != 0) {
            ((ScrollViewer)sender).ScrollToVerticalOffset(((ScrollViewer)sender).ExtentHeight);
        }
    }
}