using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour {
	public GameObject grappleRope;

	public bool ________________;

	public float pointAngle = 0f;
	float moveSpeed = 10f;
	float maxDistance = 10f;
	GameObject hookPosition;
	GameObject rope;

	Vector3 origin;

	// Use this for initialization
	void Start () {
		hookPosition = GameObject.Find ("HookedIndicator").transform.Find ("Sprite").gameObject;
		pointAngle = PlayerControl.instance.pointAngle * Mathf.PI / 180f;
		transform.position = hookPosition.transform.position;
		transform.eulerAngles = new Vector3 (0f, 0f, pointAngle);
		origin = transform.position;
		PlayerControl.instance.hookObj = this;
		transform.rotation = PlayerControl.instance.playerHook.transform.rotation;
		rope = Instantiate (grappleRope);
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
		}
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.tag == "Ground") {
			if (coll.gameObject.transform.parent.FindChild ("Sprite").GetComponent<SpriteRenderer> ().color == PlayerControl.instance.spRendMain.color || coll.gameObject.transform.parent.FindChild ("Sprite").GetComponent<SpriteRenderer> ().color == Color.white) {
				PlayerControl.instance.grappled = true;
				pointAngle = -Mathf.Atan2 ((PlayerControl.instance.gameObject.transform.position - transform.position).x, (PlayerControl.instance.gameObject.transform.position - transform.position).y) - 90 * Mathf.PI / 180f;
			}
		}
	}

	public void DestroyAll() {
		Destroy (rope);
		PlayerControl.instance.grappling = false;
		PlayerControl.instance.grappled = false;
		Destroy (this.gameObject);
	}


}
