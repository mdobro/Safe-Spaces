﻿using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour {
	public GameObject grappleRope;
	public static Grapple instance;
	AudioSource grappleHitAudio, grappleShootAudio;

	public bool ________________;

	public float pointAngle = 0f;
	float moveSpeed = 12f;
	float maxDistance = 10f;
	GameObject hookPosition;
	GameObject rope;
	bool destroyGrapple = false;

	Vector3 origin;

	// Use this for initialization
	void Start () {
		instance = this;
		hookPosition = GameObject.Find ("HookedIndicator").transform.Find ("Sprite").gameObject;
		grappleHitAudio = GameObject.Find ("Audio").transform.Find ("GrappleHit").GetComponent<AudioSource> ();
		grappleShootAudio = GameObject.Find ("Audio").transform.Find ("GrappleShoot").GetComponent<AudioSource> ();
		pointAngle = PlayerControl.instance.pointAngle * Mathf.PI / 180f;
		transform.position = hookPosition.transform.position;
		transform.eulerAngles = new Vector3 (0f, 0f, pointAngle);
		origin = transform.position;
		PlayerControl.instance.hookObj = this;
		transform.rotation = PlayerControl.instance.playerHook.transform.rotation;
		rope = Instantiate (grappleRope);
		grappleShootAudio.Play ();
	}

	void Update () {
		// Destroy grapple and allow regrapple
		if (!XInput.x.LTDown () && destroyGrapple == false) {
			destroyGrapple = true;
		}

		if (XInput.x.LTDown () && destroyGrapple == true) {
			DestroyAll ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (PlayerControl.instance.grappled) {
			Vector3 vec1 = PlayerControl.instance.gameObject.transform.position;
			vec1 += new Vector3 (moveSpeed * Mathf.Cos (pointAngle) * Time.fixedDeltaTime, moveSpeed * Mathf.Sin (pointAngle) * Time.fixedDeltaTime, 0f);
			PlayerControl.instance.gameObject.transform.position = vec1;

			if ((transform.position - PlayerControl.instance.transform.position).magnitude > maxDistance) {
				DestroyAll();
			}
		} else {
			Vector3 vec2 = transform.position;
			vec2 += new Vector3 (moveSpeed * Mathf.Cos (pointAngle) * Time.fixedDeltaTime, moveSpeed * Mathf.Sin (pointAngle) * Time.fixedDeltaTime, 0f);
			transform.position = vec2;

			if ((transform.position - origin).magnitude > maxDistance || (transform.position - PlayerControl.instance.transform.position).magnitude > maxDistance) {
				DestroyAll();
			}

			// Set Rotation
			float hookAngle = -180f / Mathf.PI * Mathf.Atan2((transform.position - PlayerControl.instance.gameObject.transform.position).x, (transform.position - PlayerControl.instance.gameObject.transform.position).y) + 90;
			transform.eulerAngles = new Vector3(0f, 0f, hookAngle);
		}
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.tag == "Ground") {
			grappleHitAudio.Play ();
			PlayerControl.instance.grappled = true;
			PlayerControl.instance.hookSprite.enabled = false;
			pointAngle = -Mathf.Atan2 ((PlayerControl.instance.gameObject.transform.position - transform.position).x, (PlayerControl.instance.gameObject.transform.position - transform.position).y) - 90 * Mathf.PI / 180f;
		} else if (coll.gameObject.tag == "Player") {
			DestroyAll ();
		}
	}

	public void DestroyAll() {
		Destroy (rope);
		PlayerControl.instance.grappling = false;
		PlayerControl.instance.grappled = false;
		Destroy (this.gameObject);
	}


}
