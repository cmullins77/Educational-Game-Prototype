using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAnalyzer : MonoBehaviour
{

    public PaletteType palette;
    public List<Color> colors;
    public List<ColorType> types;
    // Start is called before the first frame update
    void Start()
    {
        analyzePalette(colors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void analyzePalette(List<Color> cols) {
        bool paletteFound = false;
        palette = PaletteType.None;
        types = new List<ColorType>();
        foreach (Color col in cols) {
            ColorType colorType = getColor(col);
            if (!types.Contains(colorType)) {
                types.Add(colorType);
            }
        }
        if (types.Count == 1) {
            palette = PaletteType.Monochromatic;
            paletteFound = true;
        } else if (types.Count == 2) {
            if (checkComplement(types[0], types[1])) {
                palette = PaletteType.Complement;
                paletteFound = true;
            } else if (checkNearComplement(types[0], types[1])) {
                palette = PaletteType.NearComplement;
                paletteFound = true;
            }
        } else if (types.Count == 3) {
            if (checkSplitComplement(types[0], types[1], types[2])) {
                palette = PaletteType.SplitComplement;
                paletteFound = true;
            }
            else if (checkTriadic(types[0], types[1], types[2])) {
                palette = PaletteType.Triadic;
                paletteFound = true;
            }
        } else if (types.Count == 4) {
            if (checkTetradic(types[0], types[1], types[2], types[3])) {
                palette = PaletteType.Tetradic;
                paletteFound = true;
            }
        }

        if (!paletteFound) {
            if (checkAnalagous(types)) {
                palette = PaletteType.Analogous;
            }
        }
    }

    public bool checkAnalagous(List<ColorType> colTypes) {
        return (colTypes.Contains(ColorType.Red) && !colTypes.Contains(ColorType.Green) && !colTypes.Contains(ColorType.PurpleBlue) && !colTypes.Contains(ColorType.Blue) && !colTypes.Contains(ColorType.BlueGreen) && !colTypes.Contains(ColorType.GreenYellow)) ||
            (colTypes.Contains(ColorType.YellowRed) && !colTypes.Contains(ColorType.Purple) && !colTypes.Contains(ColorType.PurpleBlue) && !colTypes.Contains(ColorType.Blue) && !colTypes.Contains(ColorType.BlueGreen) && !colTypes.Contains(ColorType.Green)) ||
            (colTypes.Contains(ColorType.Yellow) && !colTypes.Contains(ColorType.RedPurple) && !colTypes.Contains(ColorType.Purple) && !colTypes.Contains(ColorType.PurpleBlue) && !colTypes.Contains(ColorType.Blue) && !colTypes.Contains(ColorType.BlueGreen)) ||
            (colTypes.Contains(ColorType.GreenYellow) && !colTypes.Contains(ColorType.Red) && !colTypes.Contains(ColorType.RedPurple) && !colTypes.Contains(ColorType.Purple) && !colTypes.Contains(ColorType.PurpleBlue) && !colTypes.Contains(ColorType.Blue)) ||
            (colTypes.Contains(ColorType.Green) && !colTypes.Contains(ColorType.YellowRed) && !colTypes.Contains(ColorType.Red) && !colTypes.Contains(ColorType.RedPurple) && !colTypes.Contains(ColorType.Purple) && !colTypes.Contains(ColorType.PurpleBlue)) ||
            (colTypes.Contains(ColorType.BlueGreen) && !colTypes.Contains(ColorType.Yellow) && !colTypes.Contains(ColorType.YellowRed) && !colTypes.Contains(ColorType.Red) && !colTypes.Contains(ColorType.RedPurple) && !colTypes.Contains(ColorType.Purple)) ||
            (colTypes.Contains(ColorType.Blue) && !colTypes.Contains(ColorType.GreenYellow) && !colTypes.Contains(ColorType.Yellow) && !colTypes.Contains(ColorType.YellowRed) && !colTypes.Contains(ColorType.Red) && !colTypes.Contains(ColorType.RedPurple)) ||
            (colTypes.Contains(ColorType.PurpleBlue) && !colTypes.Contains(ColorType.Green) && !colTypes.Contains(ColorType.GreenYellow) && !colTypes.Contains(ColorType.Yellow) && !colTypes.Contains(ColorType.YellowRed) && !colTypes.Contains(ColorType.Red)) ||
            (colTypes.Contains(ColorType.Purple) && !colTypes.Contains(ColorType.BlueGreen) && !colTypes.Contains(ColorType.Green) && !colTypes.Contains(ColorType.GreenYellow) && !colTypes.Contains(ColorType.Yellow) && !colTypes.Contains(ColorType.YellowRed)) ||
            (colTypes.Contains(ColorType.RedPurple) && !colTypes.Contains(ColorType.Blue) && !colTypes.Contains(ColorType.BlueGreen) && !colTypes.Contains(ColorType.Green) && !colTypes.Contains(ColorType.GreenYellow) && !colTypes.Contains(ColorType.Yellow));
    }

    public bool checkComplement(ColorType a, ColorType b) {
        return (a == ColorType.Red && b == ColorType.BlueGreen) || (b == ColorType.Red && a == ColorType.BlueGreen) ||
            (a == ColorType.YellowRed && b == ColorType.Blue) || (b == ColorType.YellowRed && a == ColorType.Blue) ||
            (a == ColorType.Yellow && b == ColorType.PurpleBlue) || (b == ColorType.Yellow && a == ColorType.PurpleBlue) ||
            (a == ColorType.GreenYellow && b == ColorType.Purple) || (b == ColorType.GreenYellow && a == ColorType.Purple) ||
            (a == ColorType.Green && b == ColorType.RedPurple) || (b == ColorType.Green && a == ColorType.RedPurple);
    }
    public bool checkNearComplement(ColorType a, ColorType b) {
        return (a == ColorType.Red && (b == ColorType.Green || b == ColorType.Blue)) ||
            (a == ColorType.YellowRed && (b == ColorType.BlueGreen || b == ColorType.PurpleBlue)) ||
            (a == ColorType.Yellow && (b == ColorType.Blue || b == ColorType.Purple)) ||
            (a == ColorType.GreenYellow && (b == ColorType.PurpleBlue || b == ColorType.RedPurple)) ||
            (a == ColorType.Green && (b == ColorType.Purple || b == ColorType.Red)) ||
            (a == ColorType.BlueGreen && (b == ColorType.RedPurple || b == ColorType.YellowRed)) ||
            (a == ColorType.Blue && (b == ColorType.Red || b == ColorType.Yellow)) ||
            (a == ColorType.PurpleBlue && (b == ColorType.YellowRed || b == ColorType.GreenYellow)) ||
            (a == ColorType.Purple && (b == ColorType.Yellow || b == ColorType.Green)) ||
            (a == ColorType.RedPurple && (b == ColorType.GreenYellow || b == ColorType.BlueGreen));
    }

    public bool checkSplitComplement(ColorType a, ColorType b, ColorType c) {
        return (a == ColorType.Red && (b == ColorType.Green && c == ColorType.Blue)) ||
            (a == ColorType.YellowRed && (b == ColorType.BlueGreen && c == ColorType.PurpleBlue)) ||
            (a == ColorType.Yellow && (b == ColorType.Blue && c == ColorType.Purple)) ||
            (a == ColorType.GreenYellow && (b == ColorType.PurpleBlue && c == ColorType.RedPurple)) ||
            (a == ColorType.Green && (b == ColorType.Purple && c == ColorType.Red)) ||
            (a == ColorType.BlueGreen && (b == ColorType.RedPurple && c == ColorType.YellowRed)) ||
            (a == ColorType.Blue && (b == ColorType.Red && c == ColorType.Yellow)) ||
            (a == ColorType.PurpleBlue && (b == ColorType.YellowRed && c == ColorType.GreenYellow)) ||
            (a == ColorType.Purple && (b == ColorType.Yellow && c == ColorType.Green)) ||
            (a == ColorType.RedPurple && (b == ColorType.GreenYellow && c == ColorType.BlueGreen));
    }

    public bool checkTriadic(ColorType a, ColorType b, ColorType c) {
        return (a == ColorType.Red && b == ColorType.GreenYellow && c == ColorType.PurpleBlue) ||
            (a == ColorType.YellowRed && b == ColorType.Green && c == ColorType.Purple) ||
            (a == ColorType.Yellow && b == ColorType.BlueGreen && c == ColorType.RedPurple) ||
            (a == ColorType.GreenYellow && b == ColorType.Blue && c == ColorType.Red) ||
            (a == ColorType.Green && b == ColorType.PurpleBlue && c == ColorType.YellowRed) ||
            (a == ColorType.BlueGreen && b == ColorType.Purple && c == ColorType.Yellow) ||
            (a == ColorType.Blue && b == ColorType.RedPurple && c == ColorType.GreenYellow) ||
            (a == ColorType.PurpleBlue && b == ColorType.Red && c == ColorType.Green) ||
            (a == ColorType.Purple && b == ColorType.YellowRed && c == ColorType.BlueGreen) ||
            (a == ColorType.RedPurple && b == ColorType.Yellow && c == ColorType.Blue);
    }

    public bool checkTetradic(ColorType a, ColorType b, ColorType c, ColorType d) {
        return (a == ColorType.Red && b == ColorType.GreenYellow && c == ColorType.BlueGreen && d == ColorType.Purple) ||
            (a == ColorType.Red && b == ColorType.GreenYellow && c == ColorType.Blue && d == ColorType.Purple) ||
            (a == ColorType.Red && b == ColorType.GreenYellow && c == ColorType.BlueGreen && d == ColorType.PurpleBlue) ||
            (a == ColorType.YellowRed && b == ColorType.Green && c == ColorType.Blue && d == ColorType.RedPurple) ||
            (a == ColorType.YellowRed && b == ColorType.Green && c == ColorType.PurpleBlue && d == ColorType.RedPurple) ||
            (a == ColorType.YellowRed && b == ColorType.Green && c == ColorType.Blue && d == ColorType.Purple) ||
            (a == ColorType.Yellow && b == ColorType.BlueGreen && c == ColorType.PurpleBlue && d == ColorType.Red) ||
            (a == ColorType.Yellow && b == ColorType.BlueGreen && c == ColorType.Purple && d == ColorType.Red) ||
            (a == ColorType.Yellow && b == ColorType.BlueGreen && c == ColorType.PurpleBlue && d == ColorType.RedPurple) ||
            (a == ColorType.GreenYellow && b == ColorType.Blue && c == ColorType.Purple && d == ColorType.YellowRed) ||
            (a == ColorType.GreenYellow && b == ColorType.Blue && c == ColorType.RedPurple && d == ColorType.YellowRed) ||
            (a == ColorType.GreenYellow && b == ColorType.Blue && c == ColorType.Purple && d == ColorType.Red) ||
            (a == ColorType.Green && b == ColorType.PurpleBlue && c == ColorType.RedPurple && d == ColorType.Yellow) ||
            (a == ColorType.Green && b == ColorType.PurpleBlue && c == ColorType.Red && d == ColorType.Yellow) ||
            (a == ColorType.Green && b == ColorType.PurpleBlue && c == ColorType.RedPurple && d == ColorType.YellowRed) ||
            (a == ColorType.BlueGreen && b == ColorType.Purple && c == ColorType.Red && d == ColorType.GreenYellow) ||
            (a == ColorType.BlueGreen && b == ColorType.Purple && c == ColorType.YellowRed && d == ColorType.GreenYellow) ||
            (a == ColorType.BlueGreen && b == ColorType.Purple && c == ColorType.Red && d == ColorType.Yellow) ||
            (a == ColorType.Blue && b == ColorType.RedPurple && c == ColorType.YellowRed && d == ColorType.Green) ||
            (a == ColorType.Blue && b == ColorType.RedPurple && c == ColorType.Yellow && d == ColorType.Green) ||
            (a == ColorType.Blue && b == ColorType.RedPurple && c == ColorType.YellowRed && d == ColorType.GreenYellow) ||
            (a == ColorType.PurpleBlue && b == ColorType.Red && c == ColorType.Yellow && d == ColorType.BlueGreen) ||
            (a == ColorType.PurpleBlue && b == ColorType.Red && c == ColorType.GreenYellow && d == ColorType.BlueGreen) ||
            (a == ColorType.PurpleBlue && b == ColorType.Red && c == ColorType.Yellow && d == ColorType.Green) ||
            (a == ColorType.Purple && b == ColorType.YellowRed && c == ColorType.GreenYellow && d == ColorType.Blue) ||
            (a == ColorType.Purple && b == ColorType.YellowRed && c == ColorType.Green && d == ColorType.Blue) ||
            (a == ColorType.Purple && b == ColorType.YellowRed && c == ColorType.GreenYellow && d == ColorType.BlueGreen) ||
            (a == ColorType.RedPurple && b == ColorType.Yellow && c == ColorType.Green && d == ColorType.PurpleBlue) ||
            (a == ColorType.RedPurple && b == ColorType.Yellow && c == ColorType.BlueGreen && d == ColorType.PurpleBlue) ||
            (a == ColorType.RedPurple && b == ColorType.Yellow && c == ColorType.Green && d == ColorType.Blue);
    }

    public ColorType getColor(Color col) {
        float H, S, V;
        Color.RGBToHSV(col, out H, out S, out V);
        H = H * 360;
        Debug.Log(H);
        ColorType type = ColorType.Red;
        if (H <= 20) {
            type = ColorType.Red;
        } else if (H <= 42) {
            type = ColorType.YellowRed;
        } else if (H <= 60) {
            type = ColorType.Yellow;
        } else if (H <= 85) {
            type = ColorType.GreenYellow;
        } else if (H <= 140) {
            type = ColorType.Green;
        } else if (H <= 166) {
            type = ColorType.BlueGreen;
        } else if (H <= 205) {
            type = ColorType.Blue;
        } else if (H <= 260) {
            type = ColorType.PurpleBlue;
        } else if (H <= 290) {
            type = ColorType.Purple;
        } else if (H <= 340) {
            type = ColorType.RedPurple;
        } else {
            type = ColorType.Red;
        }
        return type;
    }
}

public enum ColorType
{
    Red, YellowRed, Yellow, GreenYellow, Green, BlueGreen, Blue, PurpleBlue, Purple, RedPurple
}

public enum PaletteType
{
    None, Monochromatic, Analogous, Complement, NearComplement, SplitComplement, Triadic, Tetradic
}
