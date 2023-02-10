using System;
using System.Collections.Generic;
using MyGame.WPF.MVVM.Models.Actions;
using MyGame.WPF.MVVM.Models.Npcs;
using MyGame.WPF.MVVM.Models.Situations;
using MyGame.WPF.MVVM.Models.Situations.Home;
using Color = System.Drawing.Color;

namespace MyGame.WPF.MVVM.Models;

public class Save {
    public World World { get; set; } = new();
    public Situation Situation { get; set; } = YourBedroomSituation.Instance;
    public Npc? NpcAction { get; set; } = null;
    public string? ImagePath { get; set; }

    public List<List<Tuple<Color, string>>> SerializableTextLines { get; set; } = new();

    public List<string> ActionChoices {
        get {
            List<string> choices = new List<string>();
            foreach (string choice in Situation.GetActionChoices(World)) {
                choices.Add(choice);
            }

            return choices;
        }
    }

    public List<string> MovementsChoices {
        get {
            List<string> choices = new List<string>();
            foreach (string choice in Situation.GetMovementChoices(World)) {
                choices.Add(choice);
            }

            return choices;
        }
    }

    public List<TalkAction> PossibleTalkActions { get; set; } = new();

    public bool IsInChat { get; set; } = false;

    public bool PlayerCanAct { get; set; } = true;
    public bool TalkingBlocked { get; set; } = false;

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
}