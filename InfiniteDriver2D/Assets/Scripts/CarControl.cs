using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CarControl : MonoBehaviour {

    public float carSpeed = 5.0f;
    public Text gameOverText;
    public GameObject playAgainButton;
    public GameObject menuButton;
    Vector3 position;

	private GameObject[] musics;

	void Start () {
        playAgainButton.SetActive(false);
        menuButton.SetActive(false);
        Time.timeScale = 1;
        position = transform.position;
	}
	
	void Update () {

        // Vairavimas
        if ((Input.GetAxis("Horizontal") != 0.0f) && (Input.GetAxis("Vertical") != 0.0f)) {
            position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime * 0.8f;
            position.y += Input.GetAxis("Vertical") * carSpeed * Time.deltaTime * 0.8f;
        }
        else {
            position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime;
            position.y += Input.GetAxis("Vertical") * carSpeed * Time.deltaTime;
        }
        
        position.y = Mathf.Clamp(position.y, -2.65f, 2.4f);
        transform.position = position;

    }
    
    // Susitrenkimu atveju sustabodmas zaidimas ir pateikiamas meniu
    void OnTriggerEnter2D(Collider2D nonPlayerVehicle) {
        if(nonPlayerVehicle.tag == "NonPlayer") {
            Time.timeScale = 0;

			musics = GameObject.FindGameObjectsWithTag("MusicPlayer");

			foreach(GameObject music in musics)
			{
				if (music.name == "BackgroundMusic")
				{
					music.GetComponent<AudioSource> ().Stop ();
				} 
                else if(music.name == "CarCrashSound")
				{
					music.GetComponent<AudioSource> ().PlayDelayed(0.13f);
				}
			}
            gameOverText.enabled = true;
            playAgainButton.SetActive(true);
            menuButton.SetActive(true);
        }
            
    }

}
