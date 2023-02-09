using System;
using System.Windows.Controls;

namespace MyGame.WPF.MVVM.Views;

public partial class GameView : UserControl {
    public GameView() {
        InitializeComponent();
    }
    
    private bool _autoScroll = true;

    private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e) {
        if (e.ExtentHeightChange == 0) {
            if (((ScrollViewer)sender).VerticalOffset == ((ScrollViewer)sender).ScrollableHeight) {
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