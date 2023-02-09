using System.IO;
using System.Text.Json;
using MVVMEssentials.Commands;
using MyGame.WPF.Core.Stores;

namespace MyGame.WPF.Core.Commands;

public class SaveCommand : BaseCommand {
    private readonly SaveStore _saveStore;

    public SaveCommand(SaveStore saveStore) {
        _saveStore = saveStore;
    }

    public override void Execute(object? parameter) {
        string fileName =
            $"{_saveStore.CurrentSave!.Player.Name}_{_saveStore.CurrentSave!.World.Date.Day}" +
            $"-{_saveStore.CurrentSave!.World.Date.Month}-{_saveStore.CurrentSave!.World.Date.Year}.json";
        string jsonString = JsonSerializer.Serialize(_saveStore.CurrentSave);
        File.WriteAllText($"{fileName}", jsonString);
    }
}