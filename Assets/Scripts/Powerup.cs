using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public SphereColor type;
	public GameObject PowerupParticlePrefab;

	public bool ______________;

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform.FindChild("Sprite")) {
			foreach (Transform comp in child) {
				Color color = Color.black;
				switch (type) {
				case SphereColor.blue:
					color = (Resources.Load ("Materials/SphereMatBlue") as Material).color;
					break;
				case SphereColor.green:
					color = (Resources.Load ("Materials/SphereMatGreen") as Material).color;
					break;
				case SphereColor.yellow:
					color = (Resources.Load ("Materials/SphereMatYellow") as Material).color;
					break;
				}
				comp.GetComponent<SpriteRenderer> ().color = color;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		//give player powerup
		if (coll.gameObject.tag == "Player") {
			switch (type) {
			case SphereColor.blue:
				PlayerControl.instance.allowsBlue = true;
				//PlayerControl.instance.spRend1.color = new Color (0f, 0f, 1f);
				//PlayerControl.instance.trail1.material.SetColor ("_TintColor", new Color (0f, 0f, 1f));
				break;
			case SphereColor.green:
				PlayerControl.instance.allowsGreen = true;
				//PlayerControl.instance.spRend3.color = new Color (0f, 150f/255f, 0f);
				//PlayerControl.instance.trail3.material.SetColor ("_TintColor", new Color (0f, 150f/255f, 0f));
				break;
			case SphereColor.yellow:
				PlayerControl.instance.allowsYellow = true;
				//PlayerControl.instance.spRend2.color = new Color (1f, 1f, 0f);
				//PlayerControl.instance.trail2.material.SetColor ("_TintColor", new Color (1f, 1f, 0f));
				break;
			}
			PlayerControl.instance.playerColor = type;

			// Start Particle Effect
			GameObject pe = Instantiate(PowerupParticlePrefab, transform.position, Quaternion.identity) as GameObject;
			Destroy (pe, 0.5f);

			// Change Particle Colors
			pe.GetComponent<ParticleSystem> ().startColor = PlayerControl.instance.spRendMain.color;

			Destroy (this.gameObject);
		}
	}
}
