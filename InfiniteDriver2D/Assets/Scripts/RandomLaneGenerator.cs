using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class RandomLaneGenerator : MonoBehaviour {
    
    public GameObject lane;
    public GameObject rightLane;
    public GameObject leftLane;
    public GameObject vehicleSpawner;
    public Transform LaneSpawner;
    public Renderer warningIcon;
    public GameObject road;
    public GameObject player;
    public GameObject playAgainButton;
    public GameObject menuButton;
    public Text gameOverText;
    private GameObject[] carSpawner;
    private GameObject[] roadLanes;
    private GameObject laneClone;
    private bool runDown;
    private bool removeLane;
    private Vector3 laneSpawnerCopy;
    private int ID;
    
	private GameController gController;

	private GameObject[] musics;

	void Start () {
	gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        updateLaneArray();
        runDown = false;
        removeLane = false;
        laneSpawnerCopy = new Vector3(0f, 0f, 0f);
        InvokeRepeating("doLaneAction", 2.0f, 12.0f);
    }
	
	void Update () {
        // Naujos juostos prisišliejimas prie kelio
        if (runDown == true) {
            laneClone.transform.position = Vector3.MoveTowards(laneClone.transform.position, laneSpawnerCopy, 11.6773f * Time.deltaTime);
            if (laneClone.transform.position == laneSpawnerCopy) {
                laneClone.GetComponent<TrackOffset>().enabled = true;
                runDown = false;
            }
			gController.GetSpawners ();
        }
        // Juostos šalinimas iš kelio
        if (removeLane == true && roadLanes[ID] != null) {
            roadLanes[ID].transform.position = Vector3.MoveTowards(roadLanes[ID].transform.position, laneSpawnerCopy, 11.6773f * Time.deltaTime);
            if (roadLanes[ID].transform.position == laneSpawnerCopy) {
                Destroy(roadLanes[ID]);
                removeLane = false;
            }
			gController.GetSpawners ();
        }
        // Valdoma teritorija, kurioje žaidėjas gali važiuoti
        try {
            if ((player.transform.position.x > roadLanes[lastID(roadLanes)].transform.position.x + 0.4f ) ||
               (player.transform.position.x < roadLanes[firstID(roadLanes)].transform.position.x - 0.4f ))  {
                    gameOver();
                }
            }
        catch (System.Exception) {
        }   
    }        
    
    int firstID(GameObject[] array) {
        int firstID = 0;
        for(int i = 0; i < roadLanes.Length; i++) {
            if (array[firstID].transform.position.x > array[i].transform.position.x)
                firstID = i;
            }
        return firstID;
    }
    
    int lastID(GameObject[] array) {

        int lastID = 0;
        for(int i = 0; i < roadLanes.Length; i++) {
            if (array[lastID].transform.position.x < array[i].transform.position.x)
                lastID = i;
            }
        return lastID;
    }    
    
    /* Iš pradžių sukuriamas mašinų spawneris. laneSpawnerCopy nukopijuoja laneSpawner koordinates, 
       iš kurių nauja juosta prisišlies prie kelio. Kad naujos juostos prisišliejimas atrodytų tolygus,
       išjungiamas juostos sukimasis bei apsukami įvažiavimų spritai "entries".  */
    void spawnLane(string side) {
            if (roadLanes.Length > 6)
                doLaneAction();
            else {
                Instantiate(vehicleSpawner, LaneSpawner.position, LaneSpawner.rotation);
                LaneSpawner.transform.Translate(0, 4, 0);
                laneSpawnerCopy = new Vector3(LaneSpawner.position.x, 0.1f, LaneSpawner.position.z);
                if (side == "right") {
                    laneClone = (GameObject)Instantiate(rightLane, LaneSpawner.position, LaneSpawner.rotation);
                    laneClone.transform.parent = road.transform;
                    SpriteRenderer[] entries = laneClone.GetComponentsInChildren<SpriteRenderer>();
                    entries[0].flipX = true;
                    entries[1].flipX = true;
                }
                else if (side == "left") {
                    laneClone = (GameObject)Instantiate(leftLane, LaneSpawner.position, LaneSpawner.rotation);
                }
                laneClone.GetComponent<TrackOffset>().enabled = false;
                runDown = true;
            }
    }
    /* Naikinamos juostos ID išsaugomas Update funkcijoje esančiam ciklui panaudoti. Iš pradžių sunaikinamas
    carSpawner, kad neliktų mašinų, kai bus panaikinta juosta. Po to įjungiamas įspėjimas žaidėjui, perspėjantis 
    apie išnykstančią juostą. Pasibaigus įspėjimui, juosta nuleidžiama žemyn ir sunaikinama.*/
    IEnumerator destroyLane(int laneID, int carSpawnerID) {
            if (roadLanes[laneID].name == "MiddleLane") {
                doLaneAction();
                yield break;
            }
            if (laneID == lastID(roadLanes)) {
                SpriteRenderer[] entries = roadLanes[laneID].GetComponentsInChildren<SpriteRenderer>();
                entries[0].flipX = true;
                entries[1].flipX = true; 
            }
            ID = laneID;
            Destroy(carSpawner[carSpawnerID]);
            var iconClone = (GameObject)Instantiate(warningIcon, roadLanes[laneID].transform.position + new Vector3(0.0f, 0.0f, -1.0f), roadLanes[laneID].transform.rotation);
            Renderer icon = iconClone.GetComponent<Renderer>();
            StartCoroutine(blink(icon));
            yield return new WaitForSeconds(5.5f);
            laneSpawnerCopy = new Vector3(roadLanes[laneID].transform.position.x, -9f, roadLanes[laneID].transform.position.z);
            roadLanes[laneID].GetComponent<TrackOffset>().enabled = false;
            removeLane = true;
            yield return new WaitForSeconds(2f);
            Destroy(iconClone);
            updateLaneArray();
            Invoke("updateLaneArray", 0.1f);
    }
    // Naikinamos juostos įspėjamojo ženklo mirksėjimas
    IEnumerator blink(Renderer icon) {
        icon.enabled = !icon.enabled;
        yield return new WaitForSeconds(0.75f);
        icon.enabled = !icon.enabled;
        yield return new WaitForSeconds(0.75f);
        icon.enabled = !icon.enabled;
        yield return new WaitForSeconds(0.75f);
        icon.enabled = !icon.enabled;
	yield return new WaitForSeconds(0.75f);
	icon.enabled = !icon.enabled;
	yield return new WaitForSeconds(0.75f);
	icon.enabled = !icon.enabled;
        yield return new WaitForSeconds(1f);
        icon.enabled = !icon.enabled;
    }
    
    /* Funkcija kelio juostų bei mašinų spawninimų masyvui atnaujinti. Funkcija invoke'inima kiekvieną
    kartą, kai sunaikinama ar sukuriama nauja juosta. */
     
    void updateLaneArray() {
        carSpawner = GameObject.FindGameObjectsWithTag("VehicleSpawner");
        roadLanes = GameObject.FindGameObjectsWithTag("Lane");
    }
    
    void doLaneAction() {
        
        switch(Random.Range(1,6))
        {
                case 1:  // Juosta atsiranda is kaires
                    LaneSpawner.transform.position = new Vector3(roadLanes[firstID(roadLanes)].transform.position.x - 1f, 4.1f, 0f);
                    spawnLane("left");
                    break;
                case 2: // Juosta atsiranda is desines
                    LaneSpawner.transform.position = new Vector3(roadLanes[lastID(roadLanes)].transform.position.x + 1f, 4.1f, 0f);
                    spawnLane("right");
                    break;
                case 3: // Juosta panaikinama is kaires
                    if (roadLanes.Length == 3)
                        doLaneAction();
                    else {
                        StartCoroutine(destroyLane(firstID(roadLanes), firstID(carSpawner))); 
                    }
                    break;
                case 4: case 5: // Juosta panaikinama is desines            
                    if (roadLanes.Length == 3)
                        doLaneAction();
                    else {
                        StartCoroutine(destroyLane(lastID(roadLanes), lastID(carSpawner)));
                    }
                    break;
        }
        Invoke("updateLaneArray", 0.1f);
    }
    
    void gameOver() {
        Time.timeScale = 0;
	musics = GameObject.FindGameObjectsWithTag("MusicPlayer");
	foreach(GameObject music in musics)
	{
		if (music.name == "BackgroundMusic")
		{
			music.GetComponent<AudioSource> ().Stop ();
		}
	}
	playAgainButton.SetActive(true);
        menuButton.SetActive(true);
        gameOverText.enabled = true;
    }
}
