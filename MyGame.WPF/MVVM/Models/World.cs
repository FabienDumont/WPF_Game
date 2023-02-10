using System;
using System.Collections.Generic;
using MyGame.WPF.MVVM.Models.Npcs;
using MyGame.WPF.MVVM.Models.Situations.Home;

namespace MyGame.WPF.MVVM.Models; 

public class World {
    public Character Player { get; set; }
    public DateTime Date { get; set; } = new DateTime(2023, 1, 1, 8, 0, 0);
    public List<Npc> Npcs { get; set; } = new();

    public void InitializeNpcs() {
        Npc npc = new() { Name = "Normal Npc"};

        npc.Schedule.Add(
            new Tuple<string, DayOfWeek, TimeSpan, TimeSpan, bool>(
                LivingRoomSituation.Instance.LocationName, DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), false
            )
        );
                
        CustomNpc.Instance.SetupInfos();
                
        CustomNpc.Instance.Schedule.Add(
            new Tuple<string, DayOfWeek, TimeSpan, TimeSpan, bool>(
                LivingRoomSituation.Instance.LocationName, DayOfWeek.Sunday, new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), false
            )
        );

        Npcs.Add(npc);
        Npcs.Add(CustomNpc.Instance);
    }
}