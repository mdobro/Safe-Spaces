using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Notification : MonoBehaviour {

	public bool restartGame = false;

	void Start() {
		Utility.instance.paused = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (XInput.XboxA)) {
			if (restartGame) {
				SceneManager.LoadScene ("_Scene_0");
			}
			Utility.instance.paused = false;
			Destroy (this.gameObject);
		}
	}
}
