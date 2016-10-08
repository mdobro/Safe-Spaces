using UnityEngine;
using System.Collections;

public enum SphereColor {
	red, blue, green, yellow
}

public class MoveControl : MonoBehaviour {

	public float moveForce = 5f;
	public float jumpSpeed = 10f;
	public float maxSpeed = 10f;
	public float maxDrag = 10f;
	public Material[] playerMats;

	public bool ________________;

	MeshRenderer meshRend;
	Rigidbody rigid;
	public SphereColor _playerColor = SphereColor.red;

	public bool grounded = true;
	public bool walledLeft = false;
	public bool walledRight = false;
	public bool jumping = false;
	LayerMask[] groundLayerMask;

	void Start () {
		// Get components
		meshRend = GetComponent <MeshRenderer> ();
		rigid = GetComponent<Rigidbody> ();

		groundLayerMask = new LayerMask[4];
		groundLayerMask [0] = LayerMask.GetMask ("Object_Default", "Object_Red");
		groundLayerMask [1] = LayerMask.GetMask ("Object_Default", "Object_Blue");
		groundLayerMask [2] = LayerMask.GetMask ("Object_Default", "Object_Green");
		groundLayerMask [3] = LayerMask.GetMask ("Object_Default", "Object_Yellow");
	}

	void Update () {
		// Update colors
		if (Input.GetButtonDown (XInput.XboxA)) playerColor = SphereColor.green;
		if (Input.GetButtonDown (XInput.XboxB)) playerColor = SphereColor.red;
		if (Input.GetButtonDown (XInput.XboxX)) playerColor = SphereColor.blue;
		if (Input.GetButtonDown (XInput.XboxY)) playerColor = SphereColor.yellow;

		// Player jumping

		// Normal jump
		if (XInput.x.RTDown() && grounded) {
			jumping = true;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			rigid.velocity = vec;
		}

		// Wall jump left
		if (XInput.x.RTDown () && walledLeft) {
			jumping = true;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			vec.x = jumpSpeed / 1.5f;
			rigid.velocity = vec;
		}
		// Wall jump right
		if (XInput.x.RTDown () && walledRight) {
			jumping = true;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			vec.x = -jumpSpeed / 1.5f;
			rigid.velocity = vec;
		}

		// Stop jump
		if (!XInput.x.RTDown () && jumping && rigid.velocity.y > 0) {
			jumping = false;
			Vector3 vec = rigid.velocity;
			vec.y = vec.y / 3;
			rigid.velocity = vec;
		}
	}
	
	void FixedUpdate () {
		// Update grounded
		grounded = Physics.Raycast (transform.position, Vector3.down, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledLeft = Physics.Raycast (transform.position, Vector3.left, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledRight = Physics.Raycast (transform.position, Vector3.right, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);

		if (grounded) {
			jumping = false;
		}

		// Movement code
		Vector3 moveDir = new Vector3 (Input.GetAxis (XInput.XboxLStickX), 0, 0);

		if (!grounded) {
			rigid.drag = 0;
		} else {
			rigid.drag = Mathf.Lerp (maxDrag, 0, moveDir.magnitude);
		}

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