using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Chart[] charts;

    public int playerNum;

    public GameObject player1;
    public GameObject player2;

    public GameObject play1Cam;
    public GameObject play2Cam;
    public GameObject playerBody1;
    public GameObject playerBody2;

    public bool canGrab;
    // Start is called before the first frame update
    void Start()
    {
        Chart[] findCharts = FindObjectsOfType<Chart>();
        charts = findCharts;
        playerNum = 1;

        //player2.GetComponent<FirstPersonController>().enabled = false;
       // player2.GetComponent<CharacterController>().enabled = false;
       // player2.GetComponent<Grabber>().enabled = false;
       // play2Cam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            checkDone();
        }

        if (Input.GetKeyDown(KeyCode.RightControl)) {
            swapPlayers();
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int next = currentScene - 1;
            if (next < 0) {
                next = 9;
            }
            SceneManager.LoadScene(next);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int next = currentScene + 1;
            if (next > 9) {
                next = 0;
            }
            SceneManager.LoadScene(next);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    void swapPlayers() {
        if (playerNum == 1) {
            player1.GetComponent<FirstPersonController>().enabled = false;
            player1.GetComponent<CharacterController>().enabled = false;
            player1.GetComponent<Grabber>().enabled = false;
            play1Cam.SetActive(false);
            playerBody1.SetActive(true);

            player2.GetComponent<FirstPersonController>().enabled = true;
            player2.GetComponent<CharacterController>().enabled = true;
            player2.GetComponent<Grabber>().enabled = true;
            play2Cam.SetActive(true);
            playerBody2.SetActive(false);
            playerNum = 2;
        } else if (playerNum == 2) {
            player2.GetComponent<FirstPersonController>().enabled = false;
            player2.GetComponent<CharacterController>().enabled = false;
            player2.GetComponent<Grabber>().enabled = false;
            play2Cam.SetActive(false);
            playerBody2.SetActive(true);

            player1.GetComponent<FirstPersonController>().enabled = true;
            player1.GetComponent<CharacterController>().enabled = true;
            player1.GetComponent<Grabber>().enabled = true;
            play1Cam.SetActive(true);
            playerBody1.SetActive(false);
            playerNum = 1;
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
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int next = currentScene + 1;
            if (next > 9) {
                next = 0;
            }
            SceneManager.LoadScene(next);
        }
    }
}
