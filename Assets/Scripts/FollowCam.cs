using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	static public FollowCam instance;

	public float XWallOffset;
	public float YWallOffset;
	public bool boundsOn; //for testing purposes

	public bool ___________________;

	public float minX; // min Y is preset in Unity
	public Vector2 maxXY;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
		instance = this;
		GameObject currentRoom = GameObject.Find("Room_1");
		SetXYBounds (currentRoom);
	}
	
	// Update is called once per frame
	void Update () {
		//update the 
		Vector3 newPos = PlayerControl.instance.transform.position;
		newPos.y += offset.y;
		newPos.z += offset.z;
		if (boundsOn) {
			if (newPos.x > maxXY.x)
				newPos.x = maxXY.x;
			if (newPos.x < minX)
				newPos.x = minX;
			if (newPos.y > maxXY.y)
				newPos.y = maxXY.y;
		}
		transform.position = newPos;
	}

	public void SetXYBounds(GameObject room) {
		float floorCenter = room.transform.position.x;
		float floorLength = room.transform.FindChild ("Ceiling").transform.lossyScale.z;
		minX = floorCenter - floorLength / 2 + XWallOffset;
		float maxX = floorCenter + floorLength / 2 - XWallOffset;
		float maxY = room.transform.FindChild ("Wall").transform.lossyScale.y - YWallOffset;
		maxXY = new Vector2 (maxX, maxY);
	}
}
