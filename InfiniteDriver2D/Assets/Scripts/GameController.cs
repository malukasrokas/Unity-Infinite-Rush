using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[System.NonSerialized]
	public GameObject[] carsOnScreen = new GameObject[100];
	private GameObject[] carSpawners;
    	public Text scoreText;
    	public int score;
    	public int highScore = 0;
	public int carCountOnScreen = 0;
	public bool gameOver = false;

	void Start () {
	    GetSpawners ();
            highScore = PlayerPrefs.GetInt("highScore");
            score = 0;
            UpdateScore();
	}

	void Update () {
            score = (int)Time.timeSinceLevelLoad*100;
            UpdateScore();
            if (score > highScore)
                PlayerPrefs.SetInt("highScore", score);
	}
    
    void UpdateScore() {
        scoreText.text = "Score: " + score + "\nHigh Score: " + PlayerPrefs.GetInt("highScore");
    }

    public void GetSpawners() {
	carSpawners = GameObject.FindGameObjectsWithTag("VehicleSpawner");
    }
}
