using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Powerup : MonoBehaviour {

	public SphereColor type;
	public GameObject PowerupParticlePrefab;
	public bool black_powerup = false;

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
				if (black_powerup)
					color = Color.black;
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
			GameObject notification = Resources.Load ("Prefabs/Notification") as GameObject;
			Text title = notification.transform.FindChild ("Title").GetComponent<Text> ();
			Text description = notification.transform.FindChild ("Description").GetComponent<Text> ();
			switch (type) {
			case SphereColor.blue:
				PlayerControl.instance.allowsBlue = true;
				//display notification
				title.text = "Blue Powerup";
				description.text = "While blue, you can double jump. Hit the right trigger in the air to double jump.";
				break;
			case SphereColor.green:
				PlayerControl.instance.allowsGreen = true;
				//display notification
				title.text = "Green Powerup";
				description.text = "While green, you can use the grappling hook. Push the right stick in the desired direction and hit the left trigger to fire the grapple.";
				break;
			case SphereColor.yellow:
				PlayerControl.instance.allowsYellow = true;
				//display notification
				title.text = "Yellow Powerup";
				description.text = "While yellow, you can glide. Hold the right trigger in the air to glide.";
				break;
			}
			description.text += "\n\nPress A to dismiss.";
			if (black_powerup) {
				title.text = "Level Complete!";
				description.text = "You collected " + PlayerControl.instance.coinCount/2 + " coins out of " + Coin.coinCount + ".\n\nPress A to restart the level";
				notification.GetComponent<Notification> ().restartGame = true;
			}
			Instantiate (notification);
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
