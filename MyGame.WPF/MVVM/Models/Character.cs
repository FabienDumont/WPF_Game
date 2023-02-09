using System.Windows.Media;

namespace MyGame.WPF.MVVM.Models;

public class Character {
    public string Name { get; set; } = "Unknown Name";
    public int Age { get; set; } = 18;

    public int Money { get; set; } = 0;

    public CharacterStats Stats { get; set; } = new();

    public string FaceAppearance { get; set; } = "Masculine";

    public string ImagePath {
        get {
            if (FaceAppearance.Equals("Feminine")) {
                return "pack://application:,,,/Resources/Images/BaseFemale.png";
            }

            return "pack://application:,,,/Resources/Images/BaseMale.png";
        }
    }

    public Color Color {
        get {
            if (AppearanceGender.Equals("Male")) {
                return Colors.LightBlue;
            }

            if (AppearanceGender.Equals("Female")) {
                return Colors.MistyRose;
            }

            return Colors.MediumPurple;
        }
    }

    public string AppearanceGender {
        get {
            if (FaceAppearance.Equals("Feminine")) {
                return "Female";
            }

            if (FaceAppearance.Equals("Masculine")) {
                return "Male";
            }

            return "Transexual";
        }
    }
}