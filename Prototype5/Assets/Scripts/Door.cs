using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject line1;
    public GameObject line2;
    public GameObject source;
    public Spot spot;
    public GameObject door;
    public Color color;

    ColorHue goalHue;
    // Start is called before the first frame update
    void Start()
    {
        line1.GetComponent<Renderer>().material.color = color;
        line2.GetComponent<Renderer>().material.color = color;
        source.GetComponent<Renderer>().material.color = color;
        door.GetComponent<Renderer>().material.color = color;

        goalHue = FindObjectOfType<ColorAnalyzer>().getColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        bool colorInPlace = false;
        if (spot.currentObject != null) {
            Color col = spot.currentObject.GetComponent<Renderer>().material.color;
            ColorHue hue = FindObjectOfType<ColorAnalyzer>().getColor(col);
            ColorType type = FindObjectOfType<ColorAnalyzer>().getColorType(col);

            if (type.Equals(ColorType.Hue) && hue.Equals(goalHue)) {
                colorInPlace = true;
            }
        }
        if (colorInPlace) {
            door.SetActive(false);
            line2.SetActive(true);
        } else {
            door.SetActive(true);
            line2.SetActive(false);
        }
    }
}
