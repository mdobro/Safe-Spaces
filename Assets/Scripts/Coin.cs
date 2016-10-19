using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour {
	public GameObject CoinParticlePrefab;
	AudioSource coinAudio;

	public bool ________________;

	public static int coinCount = 0;
	public GameObject CoinText;

	// Use this for initialization
	void Start () {
		coinCount++;
		CoinText = GameObject.Find ("Game Overlay").transform.Find ("Coin Text").gameObject;
		coinAudio = GameObject.Find ("Audio").transform.Find ("Coin").GetComponent<AudioSource> ();
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

	void OnDestroy () {
		coinAudio.Play ();
	}
}
