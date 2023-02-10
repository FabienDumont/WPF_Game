using System;

namespace MyGame.WPF.MVVM.Models.Npcs; 

sealed class CustomNpc : Npc {
    private static Lazy<CustomNpc> _lazy = new(() => new CustomNpc());

    public static CustomNpc Instance => _lazy.Value;
    
    public void SetupInfos() {
        Name = "Custom Npc";
    }
}