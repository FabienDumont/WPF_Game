using System;
using System.Collections.Generic;
using MyGame.WPF.MVVM.Models.Talk;
using Color = System.Drawing.Color;

namespace MyGame.WPF.MVVM.Models;

public class Save {
    public World World { get; set; }

    public string LocationName { get; set; }

    public Npc? NpcAction { get; set; } = null;
    public string? ImagePath { get; set; }

    public List<List<Tuple<Color, string>>> SerializableTextLines { get; set; } = new();

    public List<SituationAction> PossibleActionChoices { get; set; } = new();

    public List<Movement> PossibleMovementChoices { get; set; } = new();

    public List<TalkAction> PossibleTalkActions { get; set; } = new();

    public bool IsInChat { get; set; } = false;

    public bool PlayerCanAct { get; set; } = true;
    public bool TalkingBlocked { get; set; } = false;

    public Save(World world) {
        World = world;
    }

    public List<Textline> GetTextLines() {
        List<Textline> textlines = new();
        foreach (List<Tuple<Color, string>> textLineSave in SerializableTextLines) {
            Textline textLine = new Textline();
            foreach (Tuple<Color, string> textPart in textLineSave) {
                textLine.TextParts.Add(
                    new Tuple<System.Windows.Media.Color, string>(
                        System.Windows.Media.Color.FromArgb(textPart.Item1.A, textPart.Item1.R, textPart.Item1.G, textPart.Item1.B), textPart.Item2
                    )
                );
            }

            textlines.Add(textLine);
        }

        return textlines;
    }

    public void SetSerializableTextLines(List<Textline> textlines) {
        foreach (Textline textLine in textlines) {
            List<Tuple<Color, string>> textLineSerializable = new List<Tuple<Color, string>>();
            foreach (Tuple<System.Windows.Media.Color, string> textPart in textLine.TextParts) {
                textLineSerializable.Add(
                    new Tuple<Color, string>(Color.FromArgb(textPart.Item1.A, textPart.Item1.R, textPart.Item1.G, textPart.Item1.B), textPart.Item2)
                );
            }

            SerializableTextLines.Add(textLineSerializable);
        }
    }

    public void AddSerializableTextLines(Textline textline) {
        List<Tuple<Color, string>> textLineSerializable = new List<Tuple<Color, string>>();
        foreach (Tuple<System.Windows.Media.Color, string> textPart in textline.TextParts) {
            textLineSerializable.Add(
                new Tuple<Color, string>(Color.FromArgb(textPart.Item1.A, textPart.Item1.R, textPart.Item1.G, textPart.Item1.B), textPart.Item2)
            );
        }

        SerializableTextLines.Add(textLineSerializable);
    }

    public void AddBlankTextline() {
        Textline textline = new();
        textline.TextParts.Add(new Tuple<System.Windows.Media.Color, string>(System.Windows.Media.Colors.White, ""));
        AddSerializableTextLines(textline);
    }
}