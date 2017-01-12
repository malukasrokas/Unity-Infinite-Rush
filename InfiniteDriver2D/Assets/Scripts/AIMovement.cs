using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

	[System.NonSerialized]
	public float minDrivingSpeed = 300;
	[System.NonSerialized]
	public float maxDrivingSpeed = 310;

	void Start ()
	{
		GetComponent<Rigidbody2D> ().velocity = Vector3.down * Random.Range(minDrivingSpeed,maxDrivingSpeed) * Time.deltaTime;
	}
}
