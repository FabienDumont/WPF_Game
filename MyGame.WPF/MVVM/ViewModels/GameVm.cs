using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using MyGame.WPF.Core.Commands;
using MyGame.WPF.Core.Helpers;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using MyGame.WPF.MVVM.Models.Talk;

namespace MyGame.WPF.MVVM.ViewModels;

public class GameVm : BaseVm {
    private readonly SaveStore _saveStore;
    private readonly StringStore _stringStore;
    private readonly INavigationService _informationNavigationService;
    public string? ImagePath => _saveStore.CurrentSave!.ImagePath;

    public DateTime Date => _saveStore.CurrentSave!.World.Date;
    public Character Player => _saveStore.CurrentSave!.World.Player;

    public string LocationName => _saveStore.CurrentSave!.LocationName;

    public ObservableCollection<Npc> NpcsInLocation => new(SituationHelper.GetNpcs(_saveStore));

    public ObservableCollection<Textline> DisplayedTextLines => new(_saveStore.CurrentSave!.GetTextLines());

    public ObservableCollection<SituationAction> ActionChoices => new(_saveStore.CurrentSave!.PossibleActionChoices);

    public ObservableCollection<Movement> MovementsChoices => new(_saveStore.CurrentSave!.PossibleMovementChoices);

    public ObservableCollection<TalkAction> PossibleTalkActions => new(_saveStore.CurrentSave!.PossibleTalkActions);

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
    public ICommand MakeChoiceActionCommand { get; set; }
    public ICommand MakeChoiceMovementCommand { get; set; }
    public ICommand EngageTalkCommand { get; set; }
    public ICommand TalkCommand { get; set; }
    public ICommand SaveGameCommand { get; set; }
    public ICommand LoadGameCommand { get; set; }
    public ICommand MainMenuNavigateCommand { get; set; }

    public GameVm(
        SaveStore saveStore, StringStore stringStore, CharacterStore characterStore, INavigationService characterNavigationService,
        INavigationService inventoryNavigationService, INavigationService mainMenuNavigationService, INavigationService informationNavigationService
    ) {
        _saveStore = saveStore;
        _stringStore = stringStore;
        _informationNavigationService = informationNavigationService;

        _saveStore.CurrentSaveChanged += OnCurrentSaveChanged;

        SetActions();

        CharacterNavigateCommand = new RelayCommand(
            parameter => {
                characterStore.CurrentCharacter = (Character)parameter!;
                characterNavigationService.Navigate();
            }
        );

        InventoryNavigateCommand = new NavigateCommand(inventoryNavigationService);
        SaveGameCommand = new SaveGameCommand(_saveStore);

        MakeChoiceActionCommand = new RelayCommand(parameter => { SituationHelper.ProceedChoiceAction(_saveStore, (SituationAction)parameter!); });
        MakeChoiceMovementCommand = new RelayCommand(parameter => { SituationHelper.ProceedChoiceMovement(_saveStore, (Movement)parameter!); });

        EngageTalkCommand = new RelayCommand(parameter => { EngageTalk((Npc)parameter!); });

        TalkCommand = new RelayCommand(parameter => { Talk((TalkAction)parameter!); });

        LoadGameCommand = new LoadGameCommand(_saveStore, stringStore, null, _informationNavigationService);
        MainMenuNavigateCommand = new NavigateCommand(mainMenuNavigationService);
    }

    private async void EngageTalk(Npc npc) {
        _saveStore.CurrentSave!.NpcAction = npc;
        _saveStore.CurrentSave!.IsInChat = true;
        await ActionHelper.HandleGreeting(_saveStore, _stringStore, _informationNavigationService);
    }

    private async void Talk(TalkAction action) {
        await ActionHelper.HandleTalk(_saveStore, _stringStore, _informationNavigationService, action);
    }

    private void SetActions() {
        SituationHelper.SetActions(_saveStore, _stringStore, _informationNavigationService);
    }

    private void OnCurrentSaveChanged() {
        if (_saveStore.IsPlaying) {
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(LocationName));
            OnPropertyChanged(nameof(DisplayedTextLines));
            OnPropertyChanged(nameof(ActionChoices));
            OnPropertyChanged(nameof(MovementsChoices));
            OnPropertyChanged(nameof(PossibleTalkActions));
            OnPropertyChanged(nameof(NpcsInLocation));
            OnPropertyChanged(nameof(NpcAction));
            OnPropertyChanged(nameof(IsInChat));
            OnPropertyChanged(nameof(IsInNpcInteraction));
            OnPropertyChanged(nameof(PlayerCanAct));
            OnPropertyChanged(nameof(ImagePath));
        }
    }
}