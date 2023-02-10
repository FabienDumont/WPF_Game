namespace MyGame.WPF.MVVM.Models.Actions; 

public class TalkActionResult {
    public string? NpcDialog { get; set; }
    public int? MinRelationship { get; set; }
    public int? MaxRelationship { get; set; }
    public int? EffectRelationship { get; set; }
    public int? AddedMinutes { get; set; }
    public bool? Success { get; set; }
}