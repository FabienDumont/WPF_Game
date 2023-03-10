using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models; 

public class Situation {
    public string LocationName { get; set; }
    public List<Movement>? MovementChoices { get; set; }
    public List<SituationAction>? ActionChoices { get; set; }
}