using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public GameObject flag;
	public float flag_raise_speed;
	private bool flagRaised = false;
	Vector3 target;

	void Start() {
		target = flag.transform.position;
		target.y += 1f;
	}

	void Update() {
		if (flagRaised) {
			flag.transform.position = Vector3.MoveTowards (flag.transform.position, target, flag_raise_speed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Player") {
			flagRaised = true;
		}
	}
}
