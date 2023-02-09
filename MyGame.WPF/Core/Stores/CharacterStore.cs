using System;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.Core.Stores; 

public class CharacterStore {
    private Character? _currentCharacter;

    public Character? CurrentCharacter {
        get => _currentCharacter;
        set {
            _currentCharacter = value;
            CurrentCharacterChanged?.Invoke();
        }
    }

    public bool IsUsed => CurrentCharacter != null;

    public event Action? CurrentCharacterChanged;

    public void StopUsing() {
        CurrentCharacter = null;
    }
}