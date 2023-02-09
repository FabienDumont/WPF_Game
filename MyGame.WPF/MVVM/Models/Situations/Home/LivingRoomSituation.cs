using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Situations.Home; 

public class LivingRoomSituation : Situation {
    private static readonly Lazy<LivingRoomSituation> Lazy = new(() => new LivingRoomSituation { LocationName = "Living Room (home)" });

    public static LivingRoomSituation Instance => Lazy.Value;
    
    public override bool IsOpen(DayOfWeek day, TimeSpan hour) {
        return true;
    }
    
    public override List<string> GetActionChoices(World world) {
        return new List<string>();
    }

    public override List<string> GetMovementChoices(World world) {
        List<string> choices = new List<string>();
        choices.Add("Your Bedroom");
        return choices;
    }
    
    public override Tuple<string, Situation> ProceedChoice(World world, string choice) {
        Tuple<string, Situation> result = new("Nothing happens.", LivingRoomSituation.Instance);

        switch (choice) {
            case "Your Bedroom":
                result = new("You enter your bedroom.", YourBedroomSituation.Instance);
                break;
        }

        return result;
    }

}