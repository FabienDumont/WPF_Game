using System.Collections.Generic;

namespace MyGame.WPF.MVVM.Models.Talk; 

public class TalkAction {
    public int Id { get; set; }
    public string? Label { get; set; }
    public bool? NeedPrevious { get; set; }
    public bool? EndConversation { get; set; }
    public string? PlayerDialog { get; set; }
    public List<TalkActionResult>? Results { get; set; } = new();
    public List<NextTalkAction> NextTalkActions { get; set; } = new();
}