using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour {

	public float curveRadius, pipeRadius;
	public int curveSegCount, pipeSegCount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private Vector3 GetPointOnTorus(float u, float v) {
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos(v));
		p.x = r * Mathf.Sin(u);
		p.y = r * Mathf.Cos(u);
		p.z = pipeRadius * Mathf.Sin(v);
		return p;
	}

	private void OnDrawGizmos () {
		float vStep = (2f * Mathf.PI) / pipeSegCount;

		for (int v = 0; v < pipeSegCount; v++) {
			Vector3 point = GetPointOnTorus(0f, v * vStep);
			Gizmos.DrawSphere(point, 0.1f);
		}
	}

	void OnTriggerEnter() {
		//move player to next room spawn point(tmp)
		PlayerControl.instance.currentRoomNumber++;
		GameObject nextRoom = GameObject.Find ("Room_" + PlayerControl.instance.currentRoomNumber);
		//find spawn point
		Transform spawnPoint = nextRoom.transform.FindChild("Spawn_Point");
		//move player to spawn point
		PlayerControl.instance.transform.position = spawnPoint.position;
		PlayerControl.instance.currentSpawnPoint = spawnPoint.position;
		//set the camera's maxXY and minXY in case the next room is bigger or smaller than the currentRoom
		FollowCam.instance.SetXYBounds(nextRoom);
	}
}
