using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ColorAnalyzer))]
public class Chart : MonoBehaviour
{
    public GameObject spotPrefab;
    public int width;
    public int height;

    float verticalSpacing = 0.55f;
    float horizontalSpacing = 0.55f;
    public List<Color> startingColors;
    public List<bool> isRandomized;
    public GameObject chipPrefab;
    public List<bool> startingColorsStuck;
    public bool randomizeStartingColors;

    public ChartType type;
    public PaletteType goalPalette;
    public ColorType goalType;
    public ColorHue goalHue;
    public Text chartText;
    public List<Color> goalColors;
    public List<ColDetails> requiredPalette;

    public List<Spot> spots;
    // Start is called before the first frame update
    void Start()
    {
        spots = new List<Spot>();

        float zStart = ((width - 1) / 2f) * horizontalSpacing * -1;
        float xStart = ((height - 1) / 2f) * verticalSpacing * -1;
        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {
                float z = zStart + (horizontalSpacing * i);
                float x = xStart + (verticalSpacing * j);
                float y = 0.07f;
                GameObject newSpot = Instantiate(spotPrefab, transform);
                newSpot.transform.rotation = transform.rotation;
                newSpot.transform.localPosition = new Vector3(x, y, z);
                spots.Add(newSpot.GetComponent<Spot>());
            }
        }
        List<Color> startCols = new List<Color>();
        for(int i = 0; i < startingColors.Count; i++) {
            if (!randomizeStartingColors || isRandomized[i]) {
                startCols.Add(startingColors[i]);
            }
        }
        if (startingColors.Count == spots.Count) {
            Debug.Log("SAME");
            for (int i = 0; i < spots.Count; i++) {
                Debug.Log(i);
                if (randomizeStartingColors && isRandomized[i]) {
                    int startingIndex = isRandomized[i] ? Random.Range(0, startCols.Count) : 0;
                    Debug.Log("Randomized: " + startingIndex);
                    Color col = startCols[startingIndex];
                    startCols.RemoveAt(startingIndex);
                    Spot spot = spots[i];
                    if (col.a == 1) {
                        GameObject newObject = Instantiate(chipPrefab);
                        newObject.GetComponent<Renderer>().material.color = col;
                        newObject.transform.position = spot.transform.position;
                        newObject.transform.rotation = spot.transform.rotation;
                        spot.currentObject = newObject;
                        newObject.GetComponent<Pickupable>().spot = spot;
                        if (startingColorsStuck[i]) {
                            spot.pickupable = false;
                        }
                    }
                } else {
                    int startingIndex = i;
                    Color col = startingColors[startingIndex];
                    startCols.Remove(col);
                    Spot spot = spots[i];
                    Debug.Log("Not Randomized: " + startingIndex);
                    if (col.a == 1) {
                        Debug.Log("Make new Chip");
                        GameObject newObject = Instantiate(chipPrefab);
                        newObject.GetComponent<Renderer>().material.color = col;
                        Debug.Log(col);
                        newObject.transform.position = spot.transform.position;
                        Debug.Log(newObject.transform.position);
                        newObject.transform.rotation = spot.transform.rotation;
                        Debug.Log(newObject.transform.rotation);
                        spot.currentObject = newObject;
                        Debug.Log(spot.currentObject);
                        newObject.GetComponent<Pickupable>().spot = spot;
                        Debug.Log("Pickupable Set");
                        if (startingColorsStuck[i]) {
                            Debug.Log("Stuck");
                            spot.pickupable = false;
                        }
                        Debug.Log("All Good");
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkDone() {
        bool done = false;
        if (type == ChartType.Palette) {
            List<Color> colors = new List<Color>();
            foreach (Spot spot in spots) {
                if (spot.currentObject == null) {
                    return false;
                } else {
                    colors.Add(spot.currentObject.GetComponent<Renderer>().material.color);
                }
            }
            PaletteType palette = GetComponent<ColorAnalyzer>().analyzePalette(colors);
            if (palette == goalPalette) {
                chartText.text = "CORRECT! This is a " + palette + " palette!";
                Debug.Log("DONE");
                return true;
            } else {
                chartText.text = "INCORRECT! This is a " + palette + " palette!";
                return false;
            }
        } else if (type == ChartType.ColorTypes) {
            List<Color> colors = new List<Color>();
            foreach (Spot spot in spots) {
                if (spot.currentObject == null) {
                    return false;
                }
                else {
                    colors.Add(spot.currentObject.GetComponent<Renderer>().material.color);
                }
            }
            if (GetComponent<ColorAnalyzer>().checkColorType(colors, goalType)) {
                chartText.text = "CORRECT! These are " + goalType + "s";
                Debug.Log("DONE");
                return true;
            }
            else {
                chartText.text = "INCORRECT!";
                return false;
            }
        } else if (type == ChartType.Hue) {
            List<Color> colors = new List<Color>();
            foreach (Spot spot in spots) {
                if (spot.currentObject == null) {
                    return false;
                }
                else {
                    colors.Add(spot.currentObject.GetComponent<Renderer>().material.color);
                }
            }
            if (GetComponent<ColorAnalyzer>().checkColorHue(colors, goalHue)) {
                chartText.text = "CORRECT! These are " + goalHue;
                Debug.Log("DONE");
                done = true;
            }
            else {
                chartText.text = "INCORRECT!";
                done = false;
            }
        } else if (type == ChartType.SpecificColors) {
            for (int i = 0; i < spots.Count; i++) {
                Spot spot = spots[i];
                if (spot.currentObject == null) {
                    chartText.text = "INCORRECT!";
                    return false;
                }
                else {
                    Color col = spot.currentObject.GetComponent<Renderer>().material.color;
                    Debug.Log(col + " " + goalColors[i]);
                    bool equal = checkColorsEqual(col, goalColors[i]);
                    Debug.Log(equal);
                    if (!checkColorsEqual(col, goalColors[i])) {
                        chartText.text = "INCORRECT!";
                        return false;
                    }
                }
            }
            chartText.text = "CORRECT!";
            return true;
        }
        else if (type == ChartType.SpecificColorsAnyOrder) {
            Debug.Log("Specific Colors");
            List<Color> colors = new List<Color>();
            foreach (Spot spot in spots) {
                if (spot.currentObject != null) {
                    colors.Add(spot.currentObject.GetComponent<Renderer>().material.color);
                }
            }
            GetComponent<ColorAnalyzer>().analyzePalette(colors);
            HashSet<ColDetails> details = GetComponent<ColorAnalyzer>().details;
            for (int i = 0; i < requiredPalette.Count; i++) {
                Debug.Log(requiredPalette[i]);
                if (!details.Contains(requiredPalette[i])) {
                    chartText.text = "INCORRECT!";
                    return false;
                }
                details.Remove(requiredPalette[i]);
            }
           
            chartText.text = "CORRECT!";
            return true;
        }

        return done;
    }

    bool checkColorsEqual(Color a, Color b) {
        return a.r > b.r - 0.01f && a.r < b.r + 0.01f && a.g > b.g - 0.01f && a.g < b.g + 0.01f && a.b > b.b - 0.01f && a.b < b.b + 0.01f;
    }
}

public enum ChartType
{
    Palette, ColorTypes, Hue, SpecificColors, SpecificColorsAnyOrder
}