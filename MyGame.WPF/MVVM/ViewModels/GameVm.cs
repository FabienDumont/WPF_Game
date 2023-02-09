using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using MyGame.WPF.MVVM.Models.Situations;

namespace MyGame.WPF.MVVM.ViewModels;

public class GameVm : BaseVm {
    private readonly SaveStore _saveStore;
    public string? ImagePath => _saveStore.CurrentSave!.ImagePath;

    public DateTime Date => _saveStore.CurrentSave!.World.Date;
    public Character Player => _saveStore.CurrentSave!.World.Player;

    public string LocationName => _saveStore.CurrentSave!.Situation.LocationName;

    public ObservableCollection<Npc> NpcsInLocation {
        get {
            ObservableCollection<Npc> npcs = new ObservableCollection<Npc>();
            if (!_saveStore.CurrentSave!.TalkingBlocked) {
                foreach (Npc npc in _saveStore.CurrentSave!.Situation.GetNpcs(_saveStore.CurrentSave!.World)) {
                    npcs.Add(npc);
                }
            }

            return npcs;
        }
    }

    public ObservableCollection<Textline> DisplayedTextLines => new(_saveStore.CurrentSave!.GetTextLines());

    public ObservableCollection<string> ActionChoices => new(_saveStore.CurrentSave!.ActionChoices);

    public ObservableCollection<string> MovementsChoices => new(_saveStore.CurrentSave!.MovementsChoices);
    public Npc? NpcAction => _saveStore.CurrentSave!.NpcAction;
    
    public bool PlayerCanAct => _saveStore.CurrentSave!.PlayerCanAct;

    public bool IsInNpcInteraction {
        get {
            if (_saveStore.CurrentSave!.IsInChat) {
                return true;
            }

            return false;
        }
    }

    public bool IsInChat => _saveStore.CurrentSave!.IsInChat;

    public ICommand CharacterNavigateCommand { get; set; }
    public ICommand InventoryNavigateCommand { get; set; }
    public ICommand MakeChoiceCommand { get; set; }
    public ICommand EngageTalkCommand { get; set; }
    public ICommand SaveGameCommand { get; set; }
    public ICommand LoadGameCommand { get; set; }
    public ICommand MainMenuNavigateCommand { get; set; }

    public GameVm(
        SaveStore saveStore, StringStore stringStore, CharacterStore characterStore, INavigationService characterNavigationService,
        INavigationService inventoryNavigationService, INavigationService mainMenuNavigationService, INavigationService informationNavigationService
    ) {
        _saveStore = saveStore;
        
        _saveStore.CurrentSaveChanged += OnCurrentSaveChanged;

        CharacterNavigateCommand = new RelayCommand(
            parameter => {
                characterStore.CurrentCharacter = (Character)parameter!;
                characterNavigationService.Navigate();
            }
        );

        InventoryNavigateCommand = new NavigateCommand(inventoryNavigationService);
        SaveGameCommand = new SaveGameCommand(_saveStore);

        MakeChoiceCommand = new MakeChoiceCommand(_saveStore);
        EngageTalkCommand = new RelayCommand(
            parameter => {
                _saveStore.CurrentSave!.NpcAction = (Npc)parameter!;
                _saveStore.CurrentSave!.IsInChat = true;
                _saveStore.Refresh();
            }
        );

        LoadGameCommand = new LoadGameCommand(_saveStore, stringStore, null, informationNavigationService);
        MainMenuNavigateCommand = new NavigateCommand(mainMenuNavigationService);
    }

    private void OnCurrentSaveChanged() {
        if (_saveStore.IsPlaying) {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(LocationName));
            OnPropertyChanged(nameof(DisplayedTextLines));
            OnPropertyChanged(nameof(ActionChoices));
            OnPropertyChanged(nameof(MovementsChoices));
            OnPropertyChanged(nameof(NpcsInLocation));
            OnPropertyChanged(nameof(NpcAction));
            OnPropertyChanged(nameof(IsInChat));
            OnPropertyChanged(nameof(IsInNpcInteraction));
            OnPropertyChanged(nameof(PlayerCanAct));
            OnPropertyChanged(nameof(ImagePath));
        }
    }
}