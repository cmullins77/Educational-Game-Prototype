
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public GameObject chip;
    public List<Color> possibleColors;
    public Spot spot;
    // Start is called before the first frame update
    void Start()
    {
        spot.placeable = false;
        GetComponent<Renderer>().material.color = possibleColors[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dispenseItem() {
        if (spot.currentObject == null) {
            int index = Random.Range(0, possibleColors.Count);
            GameObject newObject = Instantiate(chip);
            newObject.GetComponent<Renderer>().material.color = possibleColors[index];
            newObject.transform.parent = spot.transform;
            newObject.transform.localPosition = new Vector3(0, 0, 0);
            newObject.transform.rotation = spot.transform.rotation;
            spot.currentObject = newObject;
            newObject.GetComponent<Pickupable>().spot = spot;
        }
    }
}
