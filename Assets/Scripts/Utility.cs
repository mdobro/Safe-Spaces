using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Utility : MonoBehaviour {
	GameObject gameOverlay;
	GameObject pauseOverlay;
	Text menuItem0, menuItem1, menuItem2;

	bool _paused = false;
	bool moveMenu = true;

	static public Utility instance;


	int _menuItem = -1;


	// Use this for initialization
	void Start () {
		instance = this;
		pauseOverlay = GameObject.Find ("Pause Overlay");
		pauseOverlay.SetActive (false);
		menuItem0 = pauseOverlay.transform.Find ("Text").GetComponent<Text> ();
		menuItem1 = pauseOverlay.transform.Find ("Text (1)").GetComponent<Text> ();
		menuItem2 = pauseOverlay.transform.Find ("Text (2)").GetComponent<Text> ();
		menuItem = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(XInput.XboxStart)) {
			paused = !paused;
			if (paused) {
				pauseOverlay.SetActive (true);
				menuItem = 0;
			} else {
				pauseOverlay.SetActive (false);
			}
		}

		if (paused) {
			if (Input.GetButtonDown (XInput.XboxA)) {
				paused = false;
				pauseOverlay.SetActive (false);
				if (menuItem == 0) {
					// Do nothing
				}
				else if (menuItem == 1) {
					PlayerControl.instance.transform.position = PlayerControl.instance.currentSpawnPoint;
					PlayerControl.instance.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				} else if (menuItem == 2) {
					SceneManager.LoadScene ("_Scene_0");
				}
			}

			if (Input.GetAxis (XInput.XboxLStickY) < -0.2 && moveMenu) {
				menuItem--;

				moveMenu = false;
			}

			if (Input.GetAxis (XInput.XboxLStickY) > 0.2 && moveMenu) {
				menuItem++;

				moveMenu = false;
			}

			if (Mathf.Abs(Input.GetAxis (XInput.XboxLStickY)) < 0.2) {
				moveMenu = true;
			}
		}
	}

	public bool paused {
		get { return _paused; }
		set {
			if (value == _paused)
				return;

			switch (value) {
			case true:
				Time.timeScale = 0;
				break;
			case false:
				Time.timeScale = 1;
				break;
			}

			_paused = value;
		}
	}

	int menuItem {
		get { return _menuItem; }
		set {

			int tempVal = value;
			if (tempVal < 0)
				tempVal = 0;
			if (tempVal > 2)
				tempVal = 2;
			
			if (tempVal == _menuItem)
				return;

			switch (tempVal) {
			case 0:
				menuItem0.color = Color.yellow;
				menuItem1.color = Color.white;
				menuItem2.color = Color.white;
				break;
			case 1:
				menuItem0.color = Color.white;
				menuItem1.color = Color.yellow;
				menuItem2.color = Color.white;
				break;
			case 2:
				menuItem0.color = Color.white;
				menuItem1.color = Color.white;
				menuItem2.color = Color.yellow;
				break;
			}
			_menuItem = tempVal;
		}
	}
}
