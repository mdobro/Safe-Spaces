﻿using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public enum Powerup_type {
		Blue,
		Yellow,
		Green,
		Grapple
	}

	public Powerup_type type;

	public bool ______________;

	// Use this for initialization
	void Start () {
		switch (type) {
		case Powerup_type.Blue:
			GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatBlue") as Material;
			break;
		case Powerup_type.Green:
			GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatGreen") as Material;
			break;
		case Powerup_type.Yellow:
			GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatYellow") as Material;
			break;
		case Powerup_type.Grapple:
			//GetComponent<Renderer> ().material = Resources.Load ("Materials/") as Material;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		//give player powerup
		if (coll.gameObject.tag == "Player") {
			switch (type) {
			case Powerup_type.Blue:
				PlayerControl.instance.allowsBlue = true;
				break;
			case Powerup_type.Green:
				PlayerControl.instance.allowsGreen = true;
				break;
			case Powerup_type.Yellow:
				PlayerControl.instance.allowsYellow = true;
				break;
			case Powerup_type.Grapple:
				PlayerControl.instance.allowsGrapple = true;
				break;
			}
		}
	}
}
