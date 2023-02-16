using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models; 

public class Npc : Character {
    public string Type { get; set; } = "Random";
    public int Relationship { get; set; } = 0;

    public List<ScheduleItem> Schedule = new();

    public string? GetLocation(DateTime date) {
        foreach (ScheduleItem record in Schedule) {
            if (record.Day.Equals(date.DayOfWeek)) {
                if (date.TimeOfDay >= record.TimeBegin && date.TimeOfDay <= record.TimeEnd) {
                    return record.LocationName;
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