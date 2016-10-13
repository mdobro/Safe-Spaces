using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour {
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
			Destroy (this.gameObject);
		}
	}
}
