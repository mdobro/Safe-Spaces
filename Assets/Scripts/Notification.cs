using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour {

	void Start() {
		Utility.instance.paused = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (XInput.XboxA)) {
			Utility.instance.paused = false;
			Destroy (this.gameObject);
		}
	}
}
