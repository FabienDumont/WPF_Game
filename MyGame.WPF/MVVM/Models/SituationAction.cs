using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models; 

public class SituationAction {
    public string Label { get; set; }
    public string? NextSituation { get; set; }
    public string Text { get; set; }
    public int? AddMinutes { get; set; }
    public int? EffectEnergy { get; set; }
    public string? Special { get; set; }
    public List<AvailableTimespan>? AvailableTimespans { get; set; }
}