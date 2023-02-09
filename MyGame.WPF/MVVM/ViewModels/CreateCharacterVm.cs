using System;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using MVVMEssentials.Commands;
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
        CreateCharacterCommand = new RelayCommand(parameters => {
            Character player = new Character {
                Name = _name
            };

            saveStore.CurrentSave = new Save {
                Player = new Character {
                    Name = _name
                },
                World = new World {
                    Date = new DateTime(2023, 1, 1)
                }
            };

            new SaveCommand(saveStore).Execute(null);

            gameNavigationService.Navigate();
        });
    }
}