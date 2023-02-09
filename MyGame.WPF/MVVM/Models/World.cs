using System;
using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models; 

public class World {
    public Character Player { get; set; }
    public DateTime Date { get; set; } = new DateTime(2023, 1, 1, 8, 0, 0);
    public List<Npc> Npcs { get; set; } = new();
}