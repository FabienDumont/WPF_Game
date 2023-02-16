using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace MyGame.WPF.MVVM.Models;

public class World {
    public Character Player { get; set; }
    public DateTime Date { get; set; } = new(2023, 1, 1, 8, 0, 0);
    public List<Npc> Npcs { get; } = new();

    public World(Character player) {
        Player = player;
    }

    public void InitializeNpcs() {
        Npc npc = new() { Name = "Normal Npc" };

        string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        Stream? stream = null;

        foreach (string resource in resources) {
            if (resource.StartsWith("MyGame.WPF.Resources.JSON") && !resource.Equals("MyGame.WPF.Resources.JSON.Npc.json")) {
                stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource)!;
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();

                Npc npcToAdd = JsonConvert.DeserializeObject<Npc>(result)!;

                Npcs.Add(npcToAdd);
            }
        }

        Npcs.Add(npc);
    }
}