using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

    public static Tuple<string, Situation> ProceedChoice(SaveStore saveStore, string choice) {
        Save save = saveStore.CurrentSave!;

        return new Tuple<string, Situation>("lol", new Situation());
    }

    public static Tuple<string, string?> GetImageSituation(World world) {
        return new Tuple<string, string?>("", null);
    }

    public static void SetActions(SaveStore saveStore, StringStore stringStore, INavigationService informationNavigationService) {
        Save save = saveStore.CurrentSave!;

        try {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MyGame.WPF.Resources.JSON.Situation.{save.LocationName}.json") ??
                            throw new InvalidOperationException($"The file {save.LocationName}.json doesn't exist.");

            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();

            Situation situation = JsonConvert.DeserializeObject<Situation>(result)!;
            
            
        } catch (Exception e) {
            stringStore.CurrentString = e.Message;
            informationNavigationService.Navigate();
        }
    }
}