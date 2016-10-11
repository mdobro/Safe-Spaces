﻿using UnityEngine;
using System.Collections;

public enum SphereColor {
	red, blue, green, yellow
}

public class PlayerControl : MonoBehaviour {

	static public PlayerControl instance;

	public float moveForce = 5f;
	public float jumpSpeed = 10f;
	public float maxSpeed = 10f;
	public float maxDrag = 10f;
	public Material[] playerMats;

	public bool ________________;

	public SpriteRenderer spRendMain, spRend1, spRend2, spRend3;
	Rigidbody rigid;
	public SphereColor _playerColor = SphereColor.red;
	public int currentRoomNumber = 1;
	public Vector3 currentSpawnPoint;

	public bool grounded = true;
	public bool walledLeft = false;
	public bool walledRight = false;
	public float jumping = 0f;
	public float pointAngle = 0f;
	LayerMask[] groundLayerMask;
	SpriteRenderer hookSprite;
	GameObject playerHook;
	public bool allowsGreen = false;
	public bool allowsBlue = false;
	public bool allowsYellow = false;
	public bool allowsGrapple = false;

	void Start () {
		// Get components
		instance = this;
		spRendMain = GameObject.Find ("Player").transform.Find ("SpriteMain").GetComponent<SpriteRenderer> ();
		spRend1 = GameObject.Find ("Player").transform.Find ("Sprite1").GetComponent<SpriteRenderer> ();
		spRend2 = GameObject.Find ("Player").transform.Find ("Sprite2").GetComponent<SpriteRenderer> ();
		spRend3 = GameObject.Find ("Player").transform.Find ("Sprite3").GetComponent<SpriteRenderer> ();
		rigid = GetComponent<Rigidbody> ();
		playerHook = GameObject.Find ("HookedIndicator");
		hookSprite = playerHook.transform.Find ("Sprite").GetComponent<SpriteRenderer> ();

		groundLayerMask = new LayerMask[4];
		groundLayerMask [0] = LayerMask.GetMask ("Object_Default", "Object_Red");
		groundLayerMask [1] = LayerMask.GetMask ("Object_Default", "Object_Blue");
		groundLayerMask [2] = LayerMask.GetMask ("Object_Default", "Object_Green");
		groundLayerMask [3] = LayerMask.GetMask ("Object_Default", "Object_Yellow");

		currentSpawnPoint = transform.position;

	}

	void Update () {

		if (transform.position.y < -5) {
			//fell, respawn at currentSpawnPoint\
			transform.position = currentSpawnPoint;
		}
		// Update colors
		if (Input.GetButtonDown (XInput.XboxA) && allowsGreen) playerColor = SphereColor.green;
		if (Input.GetButtonDown (XInput.XboxB)) playerColor = SphereColor.red;
		if (Input.GetButtonDown (XInput.XboxX) && allowsBlue) playerColor = SphereColor.blue;
		if (Input.GetButtonDown (XInput.XboxY) && allowsYellow) playerColor = SphereColor.yellow;

		//////////Player jumping//////////
		// Normal jump
		if (XInput.x.RTDown() && grounded) {
			jumping = 1;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			rigid.velocity = vec;
		}
			
		// Double jump if blue
		if (!XInput.x.RTDown () && jumping == 1) {
			jumping = 2;
		}
		if (XInput.x.RTDown () && jumping == 2 && !grounded && !walledLeft && !walledRight && playerColor == SphereColor.blue) {
			jumping = 3;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			rigid.velocity = vec;
		}

		// Wall jump left
		if (XInput.x.RTDown () && walledLeft) {
			jumping = 1;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed / 1.155f;
			if (vec.x < jumpSpeed / 3f) {
				vec.x = jumpSpeed / 3f;
			} else {
				vec.x = -vec.x;
			}
			rigid.velocity = vec;
		}

		// Wall jump right
		if (XInput.x.RTDown () && walledRight) {
			jumping = 1;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			if (vec.x > -jumpSpeed / 3f) {
				vec.x = -jumpSpeed / 3f;
			} else {
				vec.x = -vec.x;
			}
			rigid.velocity = vec;
		}

		// Stop jump
		if (!XInput.x.RTDown () && jumping > 0 && rigid.velocity.y > 0) {
			Vector3 vec = rigid.velocity;
			vec.y = vec.y / 3;
			rigid.velocity = vec;
		}

		//////////Hook Indicator////////// 
		// Get indicator angle
		if (Mathf.Abs(Input.GetAxis (XInput.XboxRStickX)) > 0.05f || Mathf.Abs(Input.GetAxis (XInput.XboxRStickY)) > 0.05f) {
			pointAngle = 180f / Mathf.PI * Mathf.Atan2(Input.GetAxis(XInput.XboxRStickX), Input.GetAxis(XInput.XboxRStickY)) - 90;
			hookSprite.enabled = true;
		} else {
			hookSprite.enabled = false;
		}

		// Rotate indicator
		playerHook.transform.eulerAngles = new Vector3 (0f, 0f, pointAngle);
	}
	
	void FixedUpdate () {
		// Update grounded
		grounded = Physics.Raycast (transform.position, Vector3.down, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledLeft = Physics.Raycast (transform.position, Vector3.left, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledRight = Physics.Raycast (transform.position, Vector3.right, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);

		if (grounded) {
			jumping = 0;
		}

		//////////Player Movement//////////
		Vector3 moveDir = new Vector3 (Input.GetAxis (XInput.XboxLStickX), 0, 0);
		if (!grounded) {
			rigid.drag = 0;
		} else {
			rigid.drag = Mathf.Lerp (maxDrag, 0, moveDir.magnitude);
		}
		float forceApp = 0;
		if ((rigid.velocity.x > 0 && moveDir.x < 0) || (rigid.velocity.x < 0 && moveDir.x > 0)) {
			forceApp = 1;
		} else if ((walledLeft && moveDir.x < 0) || (walledRight && moveDir.x > 0)) {
			forceApp = 0;
		} else {
			forceApp = Mathf.Clamp ((maxSpeed - Mathf.Abs(GetComponent<Rigidbody> ().velocity.x)) / maxSpeed, 0, 1);
		}
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
					spRendMain.color = new Color (1f, 0f, 0f);
					gameObject.layer = 8;
					break;
				case SphereColor.blue:
					spRendMain.color = new Color (0f, 0f, 1f);
					gameObject.layer = 9;
					break;
				case SphereColor.green:
					spRendMain.color = new Color (0f, 1f, 0f);
					gameObject.layer = 10;
					break;
				case SphereColor.yellow:
					spRendMain.color = new Color (1f, 1f, 0f);
					gameObject.layer = 11;
					break;
				}

				_playerColor = value;
			}
		}
	}

	public void resetPlayerToCurrentSpawn() {
		transform.position = currentSpawnPoint;
	}
}