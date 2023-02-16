using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models; 

public class SituationAction {
    public string Label { get; set; }
    public List<AvailableTimespan> AvailableTimespans { get; set; }
}