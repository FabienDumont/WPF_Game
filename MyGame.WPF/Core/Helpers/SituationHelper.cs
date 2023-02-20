using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
            if (npc.GetLocation(save.World.Date) != null && npc.GetLocation(save.World.Date)!.Equals(save.Situation.LocationName)) {
                npcs.Add(npc);
            }
        }

        return npcs;
    }

    public static void SetActions(SaveStore saveStore, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;

        save.PossibleActionChoices.Clear();
        save.PossibleMovementChoices.Clear();

        if (save.Situation.MovementChoices != null) {
            foreach (Movement m in save.Situation.MovementChoices) {
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
        }

        if (save.Situation.ActionChoices != null) {
            foreach (SituationAction sa in save.Situation.ActionChoices) {
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
        }

        saveStore.Refresh();
    }

    public static async Task ProceedAction(
        SaveStore saveStore, SituationAction action, StringStore stringStore, INavigationService informationNavigationService
    ) {
        Save save = saveStore.CurrentSave!;
        save.PlayerCanAct = false;

        if (action.AddMinutes is not null) {
            save.World.Date = save.World.Date.AddMinutes((double)action.AddMinutes);
        }

        if (action.EffectEnergy is not null) {
            save.World.Player.Stats.Energy = save.World.Player.Stats.Energy + (int)action.EffectEnergy < 100
                ? save.World.Player.Stats.Energy + (int)action.EffectEnergy
                : 100;
        }
        
        Textline textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, action.Text));


        if (action.Special is not null) {
            await ProceedSpecialAction(saveStore, action.Special);
        }
        
        save.AddSerializableTextLine(textline);
        saveStore.Refresh();
        await Task.Delay(500);

        if (action.NextSituation != null) {
            save.Situation = GetSituationFromJson(action.NextSituation);
            save.SerializableTextLines.Clear();
        }

        SetActions(saveStore, stringStore, informationNavigationService);
        
        save.PlayerCanAct = true;
        saveStore.Refresh();
    }

    public static async Task ProceedMovement(SaveStore saveStore, Movement movement, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;
        save.PlayerCanAct = false;
        saveStore.Refresh();

        if (movement.NextSituation != null) {
            save.Situation = GetSituationFromJson(movement.NextSituation);
            save.SerializableTextLines.Clear();
        }
        
        Textline textline = new Textline();
        textline.TextParts.Add(new Tuple<Color, string>(Colors.White, movement.Text));
        
        save.AddSerializableTextLine(textline);
        await Task.Delay(500);

        SetActions(saveStore, stringStore, informationNavigationService);
        save.PlayerCanAct = true;
        saveStore.Refresh();
    }

    public static async Task ProceedSpecialAction(SaveStore saveStore, string specialAction) {
        Save save = saveStore.CurrentSave!;
        if (specialAction.Equals("Sleep")) {
            if (save.World.Date.TimeOfDay < new TimeSpan(8, 0, 0)) {
                save.World.Date = new DateTime(save.World.Date.Year, save.World.Date.Month, save.World.Date.Day) + new TimeSpan(8, 0, 0);
            } else {
                save.World.Date = new DateTime(save.World.Date.Year, save.World.Date.Month, save.World.Date.Day + 1) + new TimeSpan(8, 0, 0);
            }

            if (save.World.Date.DayOfWeek == DayOfWeek.Monday) {
                Textline textline = new Textline();
                if (save.World.Player.Money - 100 >= 0) {
                    save.World.Player.Money -= 100;
                    textline.TextParts.Add(new Tuple<Color, string>(Colors.White, "100$ has been spent to pay the rent."));
                    save.AddSerializableTextLine(textline);
                    await Task.Delay(500);
                } else {
                    save.World.Player.Money = 0;
                    save.GameOver = "You weren't able to pay the 100$ rent, you were kicked out of your home.";
                }
                
                saveStore.Refresh();
            }
        }
    }

    public static Situation GetSituationFromJson(string situationId) {
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MyGame.WPF.Resources.JSON.Situations.{situationId}.json") ??
                        throw new InvalidOperationException($"The file {situationId}.json doesn't exist.");

        StreamReader reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        Situation situation = JsonConvert.DeserializeObject<Situation>(result)!;

        return situation;
    }

    public static Tuple<string, string?> GetImageSituation(World world) {
        return new Tuple<string, string?>("", null);
    }
}