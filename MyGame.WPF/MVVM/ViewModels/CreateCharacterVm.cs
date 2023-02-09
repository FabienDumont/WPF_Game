using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using MyGame.WPF.MVVM.Models.Situations.Home;

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
            parameter => {
                Character player = new Character { Name = _name };

                saveStore.CurrentSave = new Save { World = { Player = new Character { Name = _name } } };
                
                Npc npc = new() { Name = "Test"};

                npc.Schedule.Add(
                    new Tuple<string, DayOfWeek, TimeSpan, TimeSpan, bool>(
                        LivingRoomSituation.Instance.LocationName, DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), false
                    )
                );

                saveStore.CurrentSave!.World.Npcs.Add(npc);
                
                Textline textline = new Textline();
                textline.TextParts.Add(new Tuple<System.Windows.Media.Color, string>(Colors.White, "You wake up."));
                List<Textline> textlines = new();
                textlines.Add(textline);
                saveStore.CurrentSave!.SetSerializableTextLines(textlines);
                
                new SaveGameCommand(saveStore).Execute(null);
                gameNavigationService.Navigate();
            }
        );
    }
}