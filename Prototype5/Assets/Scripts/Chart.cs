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

    public ChartType type;
    public PaletteType goalPalette;
    public Text chartText;

    public List<Spot> spots;
    // Start is called before the first frame update
    void Start()
    {
        spots = new List<Spot>();

        float zStart = ((width - 1) / 2f) * horizontalSpacing * -1;
        float xStart = ((height - 1) / 2f) * verticalSpacing * -1;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                float z = zStart + (horizontalSpacing * i);
                float x = xStart + (verticalSpacing * j);
                float y = 0.07f;
                GameObject newSpot = Instantiate(spotPrefab, transform);
                newSpot.transform.rotation = transform.rotation;
                newSpot.transform.localPosition = new Vector3(x, y, z);
                spots.Add(newSpot.GetComponent<Spot>());
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
        }

        return done;
    }
}

public enum ChartType
{
    Palette
}