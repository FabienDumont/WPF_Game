using System.IO;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using Newtonsoft.Json;

namespace MyGame.WPF.Core.Commands;

public class LoadGameCommand : BaseCommand {
    private readonly SaveStore _saveStore;
    private readonly StringStore _stringStore;
    private readonly INavigationService? _gameNavigationService;
    private readonly INavigationService _informationNavigationService;

    public LoadGameCommand(
        SaveStore saveStore, StringStore stringStore, INavigationService? gameNavigationService, INavigationService informationNavigationService
    ) {
        _saveStore = saveStore;
        _stringStore = stringStore;
        _gameNavigationService = gameNavigationService;
        _informationNavigationService = informationNavigationService;
    }

    public override void Execute(object? parameter) {
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        bool? result = dlg.ShowDialog();
        if (result == true) {
            Save? save;
            try {
                save = ReadFromJsonFile<Save>(dlg.FileName);
            } catch {
                save = null;
            }

            if (save != null) {
                _saveStore.CurrentSave = save;
            } else {
                _stringStore.CurrentString = $"Couldn't load this file, it isn't a valid save.";
                _informationNavigationService.Navigate();
            }

            if (_gameNavigationService != null) {
                _gameNavigationService.Navigate();
            }
        }
    }

    public static T ReadFromJsonFile<T>(string filePath) {
        T result;
        using (StreamReader file = File.OpenText(filePath)) {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            result = (T)serializer.Deserialize(file, typeof(T));
        }

        return result;
    }
}