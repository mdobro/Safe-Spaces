using UnityEngine;
using System.Collections;

public enum SphereColor {
	red, blue, green, yellow
}

public class MoveControl : MonoBehaviour {

	public float moveForce = 5f;
	public float maxSpeed = 10f;
	public float maxDrag = 10f;
	public Material[] playerMats;

	public bool ________________;

	MeshRenderer meshRend;
	Rigidbody rigid;
	public SphereColor _playerColor = SphereColor.red;


	void Start () {
		// Get components
		meshRend = GetComponent <MeshRenderer> ();
		rigid = GetComponent<Rigidbody> ();
	}

	void Update () {
		// Update colors
		if (Input.GetButtonDown ("Xbox_A")) playerColor = SphereColor.green;
		if (Input.GetButtonDown ("Xbox_B")) playerColor = SphereColor.red;
		if (Input.GetButtonDown ("Xbox_X")) playerColor = SphereColor.blue;
		if (Input.GetButtonDown ("Xbox_Y")) playerColor = SphereColor.yellow;
	}
	
	void FixedUpdate () {
		// Movement code
		Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);

		rigid.drag = Mathf.Lerp (maxDrag, 0, moveDir.magnitude);

		// Adjust drag if not grounded!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

		float forceApp = Mathf.Clamp ((maxSpeed - GetComponent<Rigidbody> ().velocity.magnitude) / maxSpeed, 0, 1);
		rigid.AddForce (moveDir * forceApp * moveForce);
	}

	public SphereColor playerColor {
		get { return _playerColor; }
		set { 	
			if (_playerColor == value)
				return;
			else {
				switch (value) {
				case SphereColor.red:
					meshRend.material = playerMats [(int)value];
					gameObject.layer = 8;
					break;
				case SphereColor.blue:
					meshRend.material = playerMats [(int)value];
					gameObject.layer = 9;
					break;
				case SphereColor.green:
					meshRend.material = playerMats [(int)value];
					gameObject.layer = 10;
					break;
				case SphereColor.yellow:
					meshRend.material = playerMats [(int)value];
					gameObject.layer = 11;
					break;
				}

				_playerColor = value;
			}
		}
	}

}