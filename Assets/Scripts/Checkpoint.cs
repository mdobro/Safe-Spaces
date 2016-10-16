using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	public GameObject flag;
	public float flag_raise_speed;
	public bool _flagRaised = false;
	public bool moveUp = false;
	public bool moveDown = false;
	Vector3 targetUp, targetDown;
	static List<GameObject> flagList = new List<GameObject> ();

	void Start() {
		flag = transform.Find ("Flag").gameObject;
		flagList.Add (this.gameObject);
		targetDown = flag.transform.position;
		targetUp = flag.transform.position;
		targetUp.y += 1;
		Debug.Log ("" + targetUp.x + " " + targetUp.y);
	}

	void Update() {
		if (moveUp) {
			flag.transform.position = Vector3.MoveTowards (flag.transform.position, targetUp, flag_raise_speed * Time.deltaTime);
			if (flag.transform.position == targetUp || moveDown) {
				moveUp = false;
			}
		}
		if (moveDown) {
			flag.transform.position = Vector3.MoveTowards (flag.transform.position, targetDown, flag_raise_speed * Time.deltaTime);
			if (flag.transform.position == targetDown || moveUp) {
				moveUp = false;
			}
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			flagRaised = true;

			for (int i = 0; i < flagList.Count; i++) {
				if (flagList [i] != this.gameObject) {
					flagList [i].GetComponent<Checkpoint> ().flagRaised = false;
				}
			}
		}
	}

	public bool flagRaised {
		get { return _flagRaised; }
		set {
			if (value == _flagRaised)
				return;

			if (value) {
				moveUp = true;
				moveDown = false;
			} else {
				moveDown = true;
				moveUp = false;
			}

			_flagRaised = value;
		}
	}
}
