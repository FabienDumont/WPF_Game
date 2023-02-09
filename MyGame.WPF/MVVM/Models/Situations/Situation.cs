using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Situations; 

public abstract class Situation {
    public string LocationName { get; set; } = "Unknown Situation";
    
    public abstract bool IsOpen(DayOfWeek day, TimeSpan hour);
    
    public abstract List<string> GetMovementChoices(World world);
    
    public abstract List<string> GetActionChoices(World world);
    
    public abstract Tuple<string, Situation> ProceedChoice(World world, string choice);
    
    public virtual Tuple<string, string?> GetImageSituation(World world) {
        string description = "";
        string imagePath = "";

        return new Tuple<string, string?>(description, imagePath);
    }

    public List<Npc> GetNpcs(World world) {
        List<Npc> npcs = new List<Npc>();
        foreach (Npc npc in world.Npcs) {
            if (npc.GetLocation(world.Date) != null && npc.GetLocation(world.Date)!.Item1.Equals(LocationName)) {
                npcs.Add(npc);
            }
        }
        return npcs;
    }
}