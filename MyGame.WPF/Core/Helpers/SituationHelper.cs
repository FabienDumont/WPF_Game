using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using MVVMEssentials.Services;
using MyGame.WPF.Core.Stores;
using MyGame.WPF.MVVM.Models;
using Newtonsoft.Json;

namespace MyGame.WPF.Core.Helpers;

public static class SituationHelper {
    public static List<Npc> GetNpcs(SaveStore saveStore) {
        Save save = saveStore.CurrentSave!;

        List<Npc> npcs = new();

        foreach (Npc npc in save.World.Npcs) {
            if (npc.GetLocation(save.World.Date) != null && npc.GetLocation(save.World.Date)!.Equals(save.LocationName)) {
                npcs.Add(npc);
            }
        }

        return npcs;
    }
    
        public static void SetActions(SaveStore saveStore, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;
        
        save.PossibleActionChoices.Clear();
        save.PossibleMovementChoices.Clear();

        try {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MyGame.WPF.Resources.JSON.Situations.{save.LocationName}.json") ??
                            throw new InvalidOperationException($"The file {save.LocationName}.json doesn't exist.");

            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();

            Situation situation = JsonConvert.DeserializeObject<Situation>(result)!;
            
            foreach (Movement m in situation.MovementChoices) {
                if (m.AvailableTimespans is not null) {
                    foreach (AvailableTimespan at in m.AvailableTimespans) {
                        if (at.Day is null || save.World.Date.DayOfWeek == at.Day) {
                            if (save.World.Date.TimeOfDay >= at.TimeBegin && save.World.Date.TimeOfDay <= at.TimeEnd) {
                                save.PossibleMovementChoices.Add(m);
                            }
                        } 
                    }
                } else {
                    save.PossibleMovementChoices.Add(m);
                }
            }

            foreach (SituationAction sa in situation.ActionChoices) {
                if (sa.AvailableTimespans is not null) {
                    foreach (AvailableTimespan at in sa.AvailableTimespans) {
                        if (at.Day is null || save.World.Date.DayOfWeek == at.Day) {
                            if (save.World.Date.TimeOfDay >= at.TimeBegin && save.World.Date.TimeOfDay <= at.TimeEnd) {
                                save.PossibleActionChoices.Add(sa);
                            }
                        } 
                    }
                } else {
                    save.PossibleActionChoices.Add(sa);
                }
            }

            saveStore.Refresh();
        } catch (Exception e) {
            stringStore.CurrentString = e.Message;
            informationNavigationService.Navigate();
        }
    }

    public static void ProceedChoiceAction(SaveStore saveStore, SituationAction choice, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;

        if (choice.AddMinutes is not null) {
            save.World.Date = save.World.Date.AddMinutes((double)choice.AddMinutes);
        }

        if (choice.EffectEnergy is not null) {
            save.World.Player.Stats.Energy =
                save.World.Player.Stats.Energy + (int) choice.EffectEnergy < 100 ? save.World.Player.Stats.Energy + (int) choice.EffectEnergy : 100;
        }

        if (choice.Special is not null) {
            ProceedSpecialAction(save, choice.Special);
        }

        Textline textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, choice.Text));

        save.AddSerializableTextLine(textline);

        if (choice.NextSituation is not null) {
            save.LocationName = choice.NextSituation;
        }

        SetActions(saveStore, stringStore, informationNavigationService);
    }

    public static void ProceedChoiceMovement(SaveStore saveStore, Movement choice, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;

        if (choice.NextSituation is not null) {
            save.LocationName = choice.NextSituation;
        }

        SetActions(saveStore, stringStore, informationNavigationService);
    }

    public static void ProceedSpecialAction(Save save, string specialAction) {
        if (specialAction.Equals("Sleep")) {
            if (save.World.Date.TimeOfDay < new TimeSpan(8, 0, 0)) {
                save.World.Date = new DateTime(save.World.Date.Year, save.World.Date.Month, save.World.Date.Day) + new TimeSpan(8, 0, 0);
            } else {
                save.World.Date = new DateTime(save.World.Date.Year, save.World.Date.Month, save.World.Date.Day + 1) + new TimeSpan(8, 0, 0);
            }
        }
    }
    
    public static Tuple<string, string?> GetImageSituation(World world) {
        return new Tuple<string, string?>("", null);
    }
}