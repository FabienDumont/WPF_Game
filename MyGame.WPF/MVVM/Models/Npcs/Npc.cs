using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Npcs; 

public class Npc : Character {
    public string Type { get; set; } = "Random";
    public int Relationship { get; set; } = 0;

    public List<Tuple<string, DayOfWeek, TimeSpan, TimeSpan, bool>> Schedule = new();

    public Tuple<string, bool>? GetLocation(DateTime date) {
        foreach (Tuple<string, DayOfWeek, TimeSpan, TimeSpan, bool> record in Schedule) {
            if (record.Item2.Equals(date.DayOfWeek)) {
                if (date.TimeOfDay >= record.Item3 && date.TimeOfDay <= record.Item4) {
                    return new Tuple<string, bool>(record.Item1, record.Item5);
                }
            }
        }

        return null;
    }
    
    public virtual string GetSituationDescription(World world) {
        string result = $"{Name} is doing something.";

        return result;
    }
}