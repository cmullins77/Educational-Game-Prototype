using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAnalyzer : MonoBehaviour
{

    public PaletteType palette;
    public List<ColorHue> types;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PaletteType analyzePalette(List<Color> cols) {
        bool paletteFound = false;
        palette = PaletteType.None;
        types = new List<ColorHue>();
        foreach (Color col in cols) {
            ColorHue colorType = getColor(col);
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
        return palette;
    }

    public bool checkColorType(List<Color> cols, ColorType goal) {
        bool correct = true;
        foreach (Color col in cols) {
            ColorType type = getColorType(col);
            if (type != goal) {
                correct = false;
            }
        }
        return correct;
    }

    public bool checkColorHue(List<Color> cols, ColorHue goal) {
        bool correct = true;
        foreach (Color col in cols) {
            ColorHue hue = getColor(col);
            if (hue != goal) {
                correct = false;
            }
        }
        return correct;
    }

    public bool checkAnalagous(List<ColorHue> colTypes) {
        return (colTypes.Contains(ColorHue.Red) && !colTypes.Contains(ColorHue.Green) && !colTypes.Contains(ColorHue.PurpleBlue) && !colTypes.Contains(ColorHue.Blue) && !colTypes.Contains(ColorHue.BlueGreen) && !colTypes.Contains(ColorHue.GreenYellow)) ||
            (colTypes.Contains(ColorHue.YellowRed) && !colTypes.Contains(ColorHue.Purple) && !colTypes.Contains(ColorHue.PurpleBlue) && !colTypes.Contains(ColorHue.Blue) && !colTypes.Contains(ColorHue.BlueGreen) && !colTypes.Contains(ColorHue.Green)) ||
            (colTypes.Contains(ColorHue.Yellow) && !colTypes.Contains(ColorHue.RedPurple) && !colTypes.Contains(ColorHue.Purple) && !colTypes.Contains(ColorHue.PurpleBlue) && !colTypes.Contains(ColorHue.Blue) && !colTypes.Contains(ColorHue.BlueGreen)) ||
            (colTypes.Contains(ColorHue.GreenYellow) && !colTypes.Contains(ColorHue.Red) && !colTypes.Contains(ColorHue.RedPurple) && !colTypes.Contains(ColorHue.Purple) && !colTypes.Contains(ColorHue.PurpleBlue) && !colTypes.Contains(ColorHue.Blue)) ||
            (colTypes.Contains(ColorHue.Green) && !colTypes.Contains(ColorHue.YellowRed) && !colTypes.Contains(ColorHue.Red) && !colTypes.Contains(ColorHue.RedPurple) && !colTypes.Contains(ColorHue.Purple) && !colTypes.Contains(ColorHue.PurpleBlue)) ||
            (colTypes.Contains(ColorHue.BlueGreen) && !colTypes.Contains(ColorHue.Yellow) && !colTypes.Contains(ColorHue.YellowRed) && !colTypes.Contains(ColorHue.Red) && !colTypes.Contains(ColorHue.RedPurple) && !colTypes.Contains(ColorHue.Purple)) ||
            (colTypes.Contains(ColorHue.Blue) && !colTypes.Contains(ColorHue.GreenYellow) && !colTypes.Contains(ColorHue.Yellow) && !colTypes.Contains(ColorHue.YellowRed) && !colTypes.Contains(ColorHue.Red) && !colTypes.Contains(ColorHue.RedPurple)) ||
            (colTypes.Contains(ColorHue.PurpleBlue) && !colTypes.Contains(ColorHue.Green) && !colTypes.Contains(ColorHue.GreenYellow) && !colTypes.Contains(ColorHue.Yellow) && !colTypes.Contains(ColorHue.YellowRed) && !colTypes.Contains(ColorHue.Red)) ||
            (colTypes.Contains(ColorHue.Purple) && !colTypes.Contains(ColorHue.BlueGreen) && !colTypes.Contains(ColorHue.Green) && !colTypes.Contains(ColorHue.GreenYellow) && !colTypes.Contains(ColorHue.Yellow) && !colTypes.Contains(ColorHue.YellowRed)) ||
            (colTypes.Contains(ColorHue.RedPurple) && !colTypes.Contains(ColorHue.Blue) && !colTypes.Contains(ColorHue.BlueGreen) && !colTypes.Contains(ColorHue.Green) && !colTypes.Contains(ColorHue.GreenYellow) && !colTypes.Contains(ColorHue.Yellow));
    }

    public bool checkComplement(ColorHue a, ColorHue b) {
        return (a == ColorHue.Red && b == ColorHue.BlueGreen) || (b == ColorHue.Red && a == ColorHue.BlueGreen) ||
            (a == ColorHue.YellowRed && b == ColorHue.Blue) || (b == ColorHue.YellowRed && a == ColorHue.Blue) ||
            (a == ColorHue.Yellow && b == ColorHue.PurpleBlue) || (b == ColorHue.Yellow && a == ColorHue.PurpleBlue) ||
            (a == ColorHue.GreenYellow && b == ColorHue.Purple) || (b == ColorHue.GreenYellow && a == ColorHue.Purple) ||
            (a == ColorHue.Green && b == ColorHue.RedPurple) || (b == ColorHue.Green && a == ColorHue.RedPurple);
    }
    public bool checkNearComplement(ColorHue a, ColorHue b) {
        return (a == ColorHue.Red && (b == ColorHue.Green || b == ColorHue.Blue)) ||
            (a == ColorHue.YellowRed && (b == ColorHue.BlueGreen || b == ColorHue.PurpleBlue)) ||
            (a == ColorHue.Yellow && (b == ColorHue.Blue || b == ColorHue.Purple)) ||
            (a == ColorHue.GreenYellow && (b == ColorHue.PurpleBlue || b == ColorHue.RedPurple)) ||
            (a == ColorHue.Green && (b == ColorHue.Purple || b == ColorHue.Red)) ||
            (a == ColorHue.BlueGreen && (b == ColorHue.RedPurple || b == ColorHue.YellowRed)) ||
            (a == ColorHue.Blue && (b == ColorHue.Red || b == ColorHue.Yellow)) ||
            (a == ColorHue.PurpleBlue && (b == ColorHue.YellowRed || b == ColorHue.GreenYellow)) ||
            (a == ColorHue.Purple && (b == ColorHue.Yellow || b == ColorHue.Green)) ||
            (a == ColorHue.RedPurple && (b == ColorHue.GreenYellow || b == ColorHue.BlueGreen));
    }

    public bool checkSplitComplement(ColorHue a, ColorHue b, ColorHue c) {
        return (a == ColorHue.Red && (b == ColorHue.Green && c == ColorHue.Blue)) ||
            (a == ColorHue.YellowRed && (b == ColorHue.BlueGreen && c == ColorHue.PurpleBlue)) ||
            (a == ColorHue.Yellow && (b == ColorHue.Blue && c == ColorHue.Purple)) ||
            (a == ColorHue.GreenYellow && (b == ColorHue.PurpleBlue && c == ColorHue.RedPurple)) ||
            (a == ColorHue.Green && (b == ColorHue.Purple && c == ColorHue.Red)) ||
            (a == ColorHue.BlueGreen && (b == ColorHue.RedPurple && c == ColorHue.YellowRed)) ||
            (a == ColorHue.Blue && (b == ColorHue.Red && c == ColorHue.Yellow)) ||
            (a == ColorHue.PurpleBlue && (b == ColorHue.YellowRed && c == ColorHue.GreenYellow)) ||
            (a == ColorHue.Purple && (b == ColorHue.Yellow && c == ColorHue.Green)) ||
            (a == ColorHue.RedPurple && (b == ColorHue.GreenYellow && c == ColorHue.BlueGreen));
    }

    public bool checkTriadic(ColorHue a, ColorHue b, ColorHue c) {
        return (a == ColorHue.Red && b == ColorHue.GreenYellow && c == ColorHue.PurpleBlue) ||
            (a == ColorHue.YellowRed && b == ColorHue.Green && c == ColorHue.Purple) ||
            (a == ColorHue.Yellow && b == ColorHue.BlueGreen && c == ColorHue.RedPurple) ||
            (a == ColorHue.GreenYellow && b == ColorHue.Blue && c == ColorHue.Red) ||
            (a == ColorHue.Green && b == ColorHue.PurpleBlue && c == ColorHue.YellowRed) ||
            (a == ColorHue.BlueGreen && b == ColorHue.Purple && c == ColorHue.Yellow) ||
            (a == ColorHue.Blue && b == ColorHue.RedPurple && c == ColorHue.GreenYellow) ||
            (a == ColorHue.PurpleBlue && b == ColorHue.Red && c == ColorHue.Green) ||
            (a == ColorHue.Purple && b == ColorHue.YellowRed && c == ColorHue.BlueGreen) ||
            (a == ColorHue.RedPurple && b == ColorHue.Yellow && c == ColorHue.Blue);
    }

    public bool checkTetradic(ColorHue a, ColorHue b, ColorHue c, ColorHue d) {
        return (a == ColorHue.Red && b == ColorHue.GreenYellow && c == ColorHue.BlueGreen && d == ColorHue.Purple) ||
            (a == ColorHue.Red && b == ColorHue.GreenYellow && c == ColorHue.Blue && d == ColorHue.Purple) ||
            (a == ColorHue.Red && b == ColorHue.GreenYellow && c == ColorHue.BlueGreen && d == ColorHue.PurpleBlue) ||
            (a == ColorHue.YellowRed && b == ColorHue.Green && c == ColorHue.Blue && d == ColorHue.RedPurple) ||
            (a == ColorHue.YellowRed && b == ColorHue.Green && c == ColorHue.PurpleBlue && d == ColorHue.RedPurple) ||
            (a == ColorHue.YellowRed && b == ColorHue.Green && c == ColorHue.Blue && d == ColorHue.Purple) ||
            (a == ColorHue.Yellow && b == ColorHue.BlueGreen && c == ColorHue.PurpleBlue && d == ColorHue.Red) ||
            (a == ColorHue.Yellow && b == ColorHue.BlueGreen && c == ColorHue.Purple && d == ColorHue.Red) ||
            (a == ColorHue.Yellow && b == ColorHue.BlueGreen && c == ColorHue.PurpleBlue && d == ColorHue.RedPurple) ||
            (a == ColorHue.GreenYellow && b == ColorHue.Blue && c == ColorHue.Purple && d == ColorHue.YellowRed) ||
            (a == ColorHue.GreenYellow && b == ColorHue.Blue && c == ColorHue.RedPurple && d == ColorHue.YellowRed) ||
            (a == ColorHue.GreenYellow && b == ColorHue.Blue && c == ColorHue.Purple && d == ColorHue.Red) ||
            (a == ColorHue.Green && b == ColorHue.PurpleBlue && c == ColorHue.RedPurple && d == ColorHue.Yellow) ||
            (a == ColorHue.Green && b == ColorHue.PurpleBlue && c == ColorHue.Red && d == ColorHue.Yellow) ||
            (a == ColorHue.Green && b == ColorHue.PurpleBlue && c == ColorHue.RedPurple && d == ColorHue.YellowRed) ||
            (a == ColorHue.BlueGreen && b == ColorHue.Purple && c == ColorHue.Red && d == ColorHue.GreenYellow) ||
            (a == ColorHue.BlueGreen && b == ColorHue.Purple && c == ColorHue.YellowRed && d == ColorHue.GreenYellow) ||
            (a == ColorHue.BlueGreen && b == ColorHue.Purple && c == ColorHue.Red && d == ColorHue.Yellow) ||
            (a == ColorHue.Blue && b == ColorHue.RedPurple && c == ColorHue.YellowRed && d == ColorHue.Green) ||
            (a == ColorHue.Blue && b == ColorHue.RedPurple && c == ColorHue.Yellow && d == ColorHue.Green) ||
            (a == ColorHue.Blue && b == ColorHue.RedPurple && c == ColorHue.YellowRed && d == ColorHue.GreenYellow) ||
            (a == ColorHue.PurpleBlue && b == ColorHue.Red && c == ColorHue.Yellow && d == ColorHue.BlueGreen) ||
            (a == ColorHue.PurpleBlue && b == ColorHue.Red && c == ColorHue.GreenYellow && d == ColorHue.BlueGreen) ||
            (a == ColorHue.PurpleBlue && b == ColorHue.Red && c == ColorHue.Yellow && d == ColorHue.Green) ||
            (a == ColorHue.Purple && b == ColorHue.YellowRed && c == ColorHue.GreenYellow && d == ColorHue.Blue) ||
            (a == ColorHue.Purple && b == ColorHue.YellowRed && c == ColorHue.Green && d == ColorHue.Blue) ||
            (a == ColorHue.Purple && b == ColorHue.YellowRed && c == ColorHue.GreenYellow && d == ColorHue.BlueGreen) ||
            (a == ColorHue.RedPurple && b == ColorHue.Yellow && c == ColorHue.Green && d == ColorHue.PurpleBlue) ||
            (a == ColorHue.RedPurple && b == ColorHue.Yellow && c == ColorHue.BlueGreen && d == ColorHue.PurpleBlue) ||
            (a == ColorHue.RedPurple && b == ColorHue.Yellow && c == ColorHue.Green && d == ColorHue.Blue);
    }

    public ColorHue getColor(Color col) {
        float H, S, V;
        Color.RGBToHSV(col, out H, out S, out V);
        H = H * 360;
        ColorHue type = ColorHue.Red;
        if (H <= 20) {
            type = ColorHue.Red;
        } else if (H <= 42) {
            type = ColorHue.YellowRed;
        } else if (H <= 60) {
            type = ColorHue.Yellow;
        } else if (H <= 85) {
            type = ColorHue.GreenYellow;
        } else if (H <= 140) {
            type = ColorHue.Green;
        } else if (H <= 166) {
            type = ColorHue.BlueGreen;
        } else if (H <= 205) {
            type = ColorHue.Blue;
        } else if (H <= 260) {
            type = ColorHue.PurpleBlue;
        } else if (H <= 290) {
            type = ColorHue.Purple;
        } else if (H <= 340) {
            type = ColorHue.RedPurple;
        } else {
            type = ColorHue.Red;
        }
        return type;
    }

    public ColorType getColorType(Color col) {
        float H, S, V;
        Color.RGBToHSV(col, out H, out S, out V);
        H = H * 360;
        ColorType type;

        float[] tintVals = {70, 70, 65, 60, 60, 50, 60, 70, 50};
        float[] shadeVals = {85, 85, 90, 90, 80, 80, 75, 80, 85};
        int hueVal = (int)getColor(col);
        if (S < 0.05f || V < 0.15f) {
            type = ColorType.Grayscale;
        } else if (S > tintVals[hueVal]/100 && V > shadeVals[hueVal]/100) {
            type = ColorType.Hue;
        } else if (S > tintVals[hueVal]/100) {
            type = ColorType.Shade;
        } else if (V > shadeVals[hueVal]/100) {
            type = ColorType.Tint;
        } else {
            type = ColorType.Tone;
        }
        return type;
    }
}

public enum ColorHue
{
    Red, YellowRed, Yellow, GreenYellow, Green, BlueGreen, Blue, PurpleBlue, Purple, RedPurple
}

public enum ColorType
{
    Hue, Tint, Tone, Shade, Grayscale
}

public enum PaletteType
{
    None, Monochromatic, Analogous, Complement, NearComplement, SplitComplement, Triadic, Tetradic
}
