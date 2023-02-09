using System;

namespace MyGame.WPF.Core.Stores;

public class StringStore {
    private string? _currentString;

    public string? CurrentString {
        get => _currentString;
        set {
            _currentString = value;
            CurrentStringChanged?.Invoke();
        }
    }

    public bool IsUsed => CurrentString != null;

    public event Action? CurrentStringChanged;

    public void StopUsing() {
        CurrentString = null;
    }
}