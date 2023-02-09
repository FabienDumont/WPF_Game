using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Situations.Home;

public class YourBedroomSituation : Situation {
    private static readonly Lazy<YourBedroomSituation> Lazy = new(() => new YourBedroomSituation { LocationName = "Your Bedroom (home)" });

    public static YourBedroomSituation Instance => Lazy.Value;

    public override bool IsOpen(DayOfWeek day, TimeSpan hour) {
        return true;
    }

    public override List<string> GetMovementChoices(World world) {
        List<string> choices = new List<string>();
        choices.Add("Living Room");
        return choices;
    }

    public override List<string> GetActionChoices(World world) {
        List<string> choices = new List<string>();
        if (world.Date.TimeOfDay < new TimeSpan(2, 0, 0) || world.Date.TimeOfDay >= new TimeSpan(8, 0, 0)) {
            choices.Add("Take a nap");
        }

        if (world.Date.TimeOfDay >= new TimeSpan(20, 0, 0) ||
            (world.Date.TimeOfDay >= new TimeSpan(0, 0, 0) && world.Date.TimeOfDay <= new TimeSpan(7, 0, 0))) {
            choices.Add("Sleep (until 8am)");
        }

        return choices;
    }

    public override Tuple<string, Situation> ProceedChoice(World world, string choice) {
        Tuple<string, Situation> result = new Tuple<string, Situation>("Nothing happens.", Instance);

        switch (choice) {
            case "Sleep (until 8am)":

                DateTime oldDate = world.Date;

                if (world.Date.TimeOfDay >= new TimeSpan(8, 0, 0)) {
                    world.Date = world.Date.AddDays(1);
                }

                world.Date = world.Date.Date + new TimeSpan(8, 0, 0);

                int hours = (int)(world.Date - oldDate).TotalHours;

                world.Player.Stats.Energy = world.Player.Stats.Energy + (hours * 10) < 100 ? world.Player.Stats.Energy + hours * 10 : 100;

                result = new Tuple<string, Situation>("You sleep until 8 am.", Instance);

                break;
            case "Take a nap":
                world.Player.Stats.Energy = world.Player.Stats.Energy + 10 < 100 ? world.Player.Stats.Energy + 10 : 100;
                world.Date = world.Date.AddHours(1);
                result = new Tuple<string, Situation>("You sleep for 1 hour.", Instance);
                break;
            case "Living Room":
                result = new Tuple<string, Situation>("You enter the living room.", LivingRoomSituation.Instance);
                break;
        }

        return result;
    }
}