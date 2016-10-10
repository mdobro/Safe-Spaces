using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public Vector2 minXY;
	public Vector2 maxXY;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//update the 
		Vector3 newPos = PlayerControl.instance.transform.position;
		newPos.y += offset.y;
		newPos.z += offset.z;
		transform.position = newPos;
	}
}
