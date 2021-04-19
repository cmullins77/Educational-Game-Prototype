using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Chart[] charts;
    // Start is called before the first frame update
    void Start()
    {
        Chart[] findCharts = FindObjectsOfType<Chart>();
        charts = findCharts;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            checkDone();
        }
    }

    void checkDone() {
        bool done = true;
        foreach(Chart chart in charts) {
            if (!chart.checkDone()) {
                done = false;
                Debug.Log("Not done");
            }
        }
        if (done) {
            Debug.Log("LEVEL DONE!");
        }
    }
}
