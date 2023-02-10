namespace MyGame.WPF.MVVM.Models.Actions; 

public class TalkActionResult {
    public string? NpcDialog { get; set; }
    public int? MinRelationship { get; set; }
    public int? MaxRelationship { get; set; }
}