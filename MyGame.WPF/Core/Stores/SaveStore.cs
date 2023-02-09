using System;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.Core.Stores; 

public class SaveStore {
    private Save? _currentSave;

    public Save? CurrentSave {
        get => _currentSave;
        set {
            _currentSave = value;
            CurrentSaveChanged?.Invoke();
        }
    }

    public bool IsUsed => CurrentSave != null;

    public event Action? CurrentSaveChanged;

    public void StopUsing() {
        CurrentSave = null;
    }
}