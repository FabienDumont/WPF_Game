using System.IO;
using System.Text.Json;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.Core.Commands;

public class LoadCommand : BaseCommand {
    private readonly SaveStore _saveStore;
    private readonly INavigationService? _gameNavigationService;

    public LoadCommand(SaveStore saveStore, INavigationService? gameNavigationService) {
        _saveStore = saveStore;
        _gameNavigationService = gameNavigationService;
    }

    public override void Execute(object? parameter) {
        string fileName = "Save.json";
        string jsonString = File.ReadAllText(fileName);
        Save save = JsonSerializer.Deserialize<Save>(jsonString)!;

        _saveStore.CurrentSave = save;

        if (_gameNavigationService != null) {
            _gameNavigationService.Navigate();
        }
    }
}