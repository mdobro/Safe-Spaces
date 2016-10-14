using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(XInput.XboxA))
			Destroy(this.gameObject);
	}
}
