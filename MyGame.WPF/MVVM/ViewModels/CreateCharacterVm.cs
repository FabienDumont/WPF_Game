using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
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
        CreateCharacterCommand = new RelayCommand(
            _ => {
                Character player = new Character { Name = _name };

                saveStore.CurrentSave = new Save(new World(player));
                
                saveStore.CurrentSave!.World.InitializeNpcs();

                Textline textline = new Textline();
                textline.TextParts.Add(new Tuple<Color, string>(Colors.White, "You wake up."));
                List<Textline> textlines = new();
                textlines.Add(textline);
                saveStore.CurrentSave!.SetSerializableTextLines(textlines);
                
                new SaveGameCommand(saveStore).Execute(null);
                gameNavigationService.Navigate();
            }
        );
    }
}