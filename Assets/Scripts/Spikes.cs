using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {
	public GameObject deathParticlePrefab;
	AudioSource deathAudio;

	// Use this for initialization
	void Start () {
		deathAudio = GameObject.Find ("Audio").transform.Find ("Death").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player") {
			// Start Particle Effect
			GameObject pe = Instantiate(deathParticlePrefab, PlayerControl.instance.transform.position, Quaternion.identity) as GameObject;
			Destroy (pe, 0.5f);
			deathAudio.Play ();
			// Change Particle Colors
			pe.GetComponent<ParticleSystem> ().startColor = PlayerControl.instance.spRendMain.color;

			PlayerControl.instance.GetComponent<Rigidbody> ().constraints = PlayerControl.instance.frozen;
			PlayerControl.instance.spRendMain.enabled = false;
			PlayerControl.instance.spRend1.enabled = false;
			PlayerControl.instance.spRend2.enabled = false;
			PlayerControl.instance.spRend3.enabled = false;
			PlayerControl.instance.trail1.enabled = false;
			PlayerControl.instance.trail2.enabled = false;
			PlayerControl.instance.trail3.enabled = false;
			PlayerControl.instance.GetComponent<PlayerControl> ().enabled = false;

			Invoke ("FinishCollision", 1f);
		}
	}

	void FinishCollision() {
		PlayerControl.instance.GetComponent<PlayerControl> ().enabled = true;
		PlayerControl.instance.GetComponent<Rigidbody> ().constraints = PlayerControl.instance.normal;
		PlayerControl.instance.spRendMain.enabled = true;
		PlayerControl.instance.spRend1.enabled = true;
		PlayerControl.instance.spRend2.enabled = true;
		PlayerControl.instance.spRend3.enabled = true;
		PlayerControl.instance.trail1.enabled = true;
		PlayerControl.instance.trail2.enabled = true;
		PlayerControl.instance.trail3.enabled = true;
		PlayerControl.instance.resetPlayerToCurrentSpawn();
	}
}
