using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	private GameObject spawningCar;

	public GameObject miniTruck;
	public GameObject[] carTypes;

	private GameController gController;
	private bool spawnPermission;

	private int numberOfLinesBetweenCars;

	private int spawnerID;


	//Atstumas tarp mašinų
	public float minSameLaneCarDist;
	public float minSideLaneCarDist;

	[System.NonSerialized]
	public float minSpawnWait = 0.5f;
	[System.NonSerialized]
	public float maxSpawnWait = 3.5f;

	void Start () {
		gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		spawnerID = (int)(this.GetComponent<Transform> ().position.x + 20.5f);

		StartCoroutine (SpawnWaves());
	}

	IEnumerator SpawnWaves()
	{
		while(!gController.gameOver)
		{
			yield return new WaitForSeconds (Random.Range (minSpawnWait, maxSpawnWait));

			if (canCarSpawn ())
			{
				spawningCar = Instantiate (carTypes [Random.Range (0, carTypes.Length)], this.transform.position, this.transform.rotation) as GameObject;
				carAddToCarsArray ();
			}
		}
	}

	void carAddToCarsArray()
	{
		if (gController.carCountOnScreen + 1 >= 100)
			gController.carCountOnScreen = 0;
			else gController.carCountOnScreen++;
		gController.carsOnScreen [spawnerID] = spawningCar;
	}

	bool canCarSpawn()
	{
		spawnPermission = true;

		if (gController.carsOnScreen[spawnerID] != null && Mathf.Abs(gController.carsOnScreen[spawnerID].GetComponent<Transform>().position.y - this.transform.position.y) < minSameLaneCarDist)
		{
			spawnPermission = false;
		}
		else if(gController.carsOnScreen [spawnerID - 1] != null &&	Mathf.Abs(gController.carsOnScreen[spawnerID - 1].GetComponent<Transform>().position.y - this.transform.position.y) < minSideLaneCarDist)
		{
			spawnPermission = false;
		}
		else if(gController.carsOnScreen [spawnerID + 1] != null &&	Mathf.Abs(gController.carsOnScreen[spawnerID + 1].GetComponent<Transform>().position.y - this.transform.position.y) < minSideLaneCarDist)
		{
			spawnPermission = false;
		}


		return spawnPermission;
	}
}
