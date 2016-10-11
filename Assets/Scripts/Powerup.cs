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
			transform.GetChild(0).GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatBlue") as Material;
			break;
		case Powerup_type.Green:
			transform.GetChild(0).GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatGreen") as Material;
			break;
		case Powerup_type.Yellow:
			transform.GetChild(0).GetComponent<Renderer> ().material = Resources.Load ("Materials/SphereMatYellow") as Material;
			break;
		case Powerup_type.Grapple:
			//transform.GetChild(0).GetComponent<Renderer> ().material = Resources.Load ("Materials/") as Material;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		//give player powerup
		if (coll.gameObject.tag == "Player") {
			Destroy (this.gameObject);
			switch (type) {
			case Powerup_type.Blue:
				PlayerControl.instance.allowsBlue = true;
				PlayerControl.instance.spRend1.color = new Color (0f, 0f, 1f);
				PlayerControl.instance.trail1.material.SetColor ("_TintColor", new Color (0f, 0f, 1f));
				break;
			case Powerup_type.Green:
				PlayerControl.instance.allowsGreen = true;
				PlayerControl.instance.spRend2.color = new Color (0f, 150f/255f, 0f);
				PlayerControl.instance.trail2.material.SetColor ("_TintColor", new Color (0f, 150f/255f, 0f));
				break;
			case Powerup_type.Yellow:
				PlayerControl.instance.allowsYellow = true;
				PlayerControl.instance.spRend3.color = new Color (1f, 1f, 0f);
				PlayerControl.instance.trail3.material.SetColor ("_TintColor", new Color (1f, 1f, 0f));
				break;
			case Powerup_type.Grapple:
				PlayerControl.instance.allowsGrapple = true;
				break;
			}
		}
	}
}
