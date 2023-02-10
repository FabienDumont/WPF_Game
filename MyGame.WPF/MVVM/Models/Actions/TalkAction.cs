using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Actions; 

public class TalkAction {
    public int Id { get; set; }
    public string? Label { get; set; }
    public bool? EndConversation { get; set; }
    public string? PlayerDialog { get; set; }
    public List<TalkActionResult>? Results { get; set; } = new();
}