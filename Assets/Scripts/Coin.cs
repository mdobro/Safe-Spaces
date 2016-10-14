﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour {
	public GameObject CoinParticlePrefab;

	public bool ________________;

	public GameObject CoinText;

	// Use this for initialization
	void Start () {
		CoinText = GameObject.Find ("Game Overlay").transform.Find ("Coin Text").gameObject;
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.tag == "Player") {
			//add coin to player
			PlayerControl.instance.coinCount++;
			CoinText.GetComponent<Text> ().text = "" + PlayerControl.instance.coinCount/2;

			// Start Particle Effect
			Object pe = Instantiate(CoinParticlePrefab, transform.position, Quaternion.identity);
			Destroy (pe, 0.5f);

			Destroy (this.gameObject);
		}
	}
}
