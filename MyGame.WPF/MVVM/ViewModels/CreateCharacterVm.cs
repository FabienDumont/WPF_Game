using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Helpers;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.MVVM.ViewModels;

public class CreateCharacterVm : BaseVm {
    private string _name = string.Empty;

    public string Name {
        get => _name;
        set {
            _name = value;
            OnPropertyChanged();
        }
    }

    public ICommand CreateCharacterCommand { get; set; }

    public CreateCharacterVm(SaveStore saveStore, INavigationService gameNavigationService) {
        
        Name = "Unknown";
        
        CreateCharacterCommand = new RelayCommand(
            _ => {
                Character player = new Character { Name = _name };

                saveStore.CurrentSave = new Save(new World(player));
                
                saveStore.CurrentSave!.World.InitializeNpcs();

                saveStore.CurrentSave!.Situation = SituationHelper.GetSituationFromJson("YourBedroom");

                Textline textline = new Textline();
                textline.TextParts.Add(new Tuple<Color, string>(Colors.White, "You wake up."));
                saveStore.CurrentSave!.AddSerializableTextLine(textline);
                
                new SaveGameCommand(saveStore).Execute(null);
                gameNavigationService.Navigate();
            }
        );
    }
}