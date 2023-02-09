using System.IO;
using MVVMEssentials.Commands;
using MyGame.WPF.Core.Stores;
using Newtonsoft.Json;

namespace MyGame.WPF.Core.Commands;

public class SaveGameCommand : BaseCommand {
    private readonly SaveStore _saveStore;

    public SaveGameCommand(SaveStore saveStore) {
        _saveStore = saveStore;
    }

    public override void Execute(object? parameter) {
        Directory.CreateDirectory("Saves");
        string folderName = "Saves/" + _saveStore.CurrentSave!.World.Player.Name;
        Directory.CreateDirectory(folderName);
        string saveName = $"{_saveStore.CurrentSave!.World.Player.Name}_{_saveStore.CurrentSave!.World.Date.Day}-" +
                          $"{_saveStore.CurrentSave!.World.Date.Month}-{_saveStore.CurrentSave!.World.Date.Year}";

        WriteToJsonFile(folderName + "/" + saveName + ".json", _saveStore.CurrentSave!);
    }

    public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) {
        JsonSerializer serializer = new JsonSerializer();
        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        serializer.NullValueHandling = NullValueHandling.Ignore;
        serializer.Formatting = Formatting.Indented;
        serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        serializer.TypeNameHandling = TypeNameHandling.Objects;

        using (StreamWriter sw = new StreamWriter(filePath))
        using (JsonWriter writer = new JsonTextWriter(sw)) {
            serializer.Serialize(writer, objectToWrite);
        }
    }
}