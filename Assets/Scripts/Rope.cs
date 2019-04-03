using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {
	GameObject hook;

	// Use this for initialization
	void Start () {
		hook = GameObject.Find ("GrapplingHook(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		// Set Position
		transform.position = (hook.transform.position + PlayerControl.instance.gameObject.transform.position) / 2;

		// Set Size
		Vector3 vec = transform.localScale;
		vec.y = (hook.transform.position - PlayerControl.instance.gameObject.transform.position).magnitude / 2;
		transform.localScale = vec;

		// Set Rotation
		float pointAngle = -180f / Mathf.PI * Mathf.Atan2((hook.transform.position - PlayerControl.instance.gameObject.transform.position).x, (hook.transform.position - PlayerControl.instance.gameObject.transform.position).y);
		transform.eulerAngles = new Vector3(0f, 0f, pointAngle);
	}
}
