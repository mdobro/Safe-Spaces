﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	static public FollowCam instance;

	public Transform        poi; // The Point Of Interest of the CameraFollow script. - JB
	public Vector3          offset = new Vector3(0,1,-10);
	public float            easingU = 0.05f;

	private Camera          cam;

	void Awake () {
		instance = this;

		cam = GetComponent<Camera>();

		// Initially position the camera exactly over the poi - JB
		transform.position = poi.position + offset;
	}

	void Update () {
		if (Input.GetButtonDown (XInput.XboxBack)) {
			if (offset.z == -20f) {
				offset.z = -40f;
			} else {
				offset.z = -20f;
			}
		}
		
	}

	// Update is called once per frame - JB
	void FixedUpdate () {
		if (poi != null) {
			Vector3 p0, p1, p01;

			p0 = transform.position;
			p1 = poi.position + offset;

			// Linear Interpolation - Look it up in Appendix B - JB
			p01 = (1 - easingU) * p0 + easingU * p1;

			p01.x = RoundToNearestPixel (p01.x, cam);
			p01.y = RoundToNearestPixel (p01.y, cam);

			transform.position = p01;
		}
	}


	// From https://www.reddit.com/r/Unity3D/comments/34ip2j/gaps_between_tiled_sprites_help/ - JB
	private float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return adjustedUnityUnits;
	}
}
