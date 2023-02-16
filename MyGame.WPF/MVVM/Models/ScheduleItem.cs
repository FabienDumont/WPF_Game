using System;

namespace MyGame.WPF.MVVM.Models;

public class ScheduleItem {
    public string LocationName { get; set; }

    public DayOfWeek Day { get; set; }

    public TimeSpan TimeBegin { get; set; }

    public TimeSpan TimeEnd { get; set; }
}