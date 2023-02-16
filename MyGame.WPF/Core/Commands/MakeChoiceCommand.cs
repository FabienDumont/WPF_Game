using System;
using System.Collections.Generic;
using System.Windows.Media;
using MVVMEssentials.Commands;
using MyGame.WPF.Core.Helpers;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;

namespace MyGame.WPF.Core.Commands; 

public class MakeChoiceCommand : BaseCommand {
    private SaveStore _saveStore;
    
    public MakeChoiceCommand(SaveStore saveStore) {
        _saveStore = saveStore;
    }

    public override void Execute(object? parameter) {
        string choice = parameter!.ToString()!;
        Tuple<string, Situation> result = SituationHelper.ProceedChoice(_saveStore, choice);
        bool newLocation = false;
        if (!result.Item2.LocationName.Equals(_saveStore.CurrentSave!.LocationName)) {
            newLocation = true;
            _saveStore.CurrentSave!.SerializableTextLines.Clear();
        }

        List<Textline> textlines = new();
        
        Textline textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, result.Item1));
        textlines.Add(textline);

        textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, ""));
        textlines.Add(textline);

        _saveStore.CurrentSave!.LocationName = result.Item2.LocationName;

        if (newLocation) {
            _saveStore.CurrentSave!.ImagePath = null;
            Tuple<string, string?> tuple = SituationHelper.GetImageSituation(_saveStore.CurrentSave!.World);
            if (!tuple.Item1.Equals("")) {
                textline = new Textline();
                textline.TextParts.Add(new Tuple<Color, string>(Colors.White, tuple.Item1));
                textlines.Add(textline);

                _saveStore.CurrentSave!.ImagePath = tuple.Item2;
                _saveStore.CurrentSave!.TalkingBlocked = true;
            } else {
                foreach (Npc npc in SituationHelper.GetNpcs(_saveStore)) {
                    textline = new Textline();
                    textline.TextParts.Add(new Tuple<Color, string>(Colors.White, tuple.Item1));
                    string test = npc.GetSituationDescription(_saveStore.CurrentSave!.World);
                    test = test.Replace(npc.Name, "|" + npc.Name + "|");
                    foreach (string part in test.Split('|')) {
                        if (!part.Equals("")) {
                            textline.TextParts.Add(
                                part.Equals(npc.Name) ? new Tuple<Color, string>(npc.Color, part) : new Tuple<Color, string>(Colors.White, part)
                            );
                        }
                    }

                    textlines.Add(textline);
                    
                }

                _saveStore.CurrentSave!.TalkingBlocked = false;
            }
        }

        textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, ""));
        textlines.Add(textline);

        _saveStore.CurrentSave!.SetSerializableTextLines(textlines);

        _saveStore.Refresh();
    }
}