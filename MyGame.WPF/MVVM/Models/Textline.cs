using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MyGame.WPF.MVVM.Models;

public class Textline {
    public List<Tuple<Color, string>> TextParts { get; set; } = new();
}