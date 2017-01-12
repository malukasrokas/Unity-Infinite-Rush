using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    
    public Transform Player;
    
	void LateUpdate () {
        this.transform.position = new Vector3(Player.position.x, 0, this.transform.position.z);
	}
}
