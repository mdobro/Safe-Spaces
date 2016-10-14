using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SphereColor {
	red, blue, green, yellow
}

public class PlayerControl : MonoBehaviour {

	static public PlayerControl instance;

	public float moveForce = 5f;
	public float jumpSpeed = 10f;
	public float maxSpeed = 10f;
	public float maxDrag = 10f;
	public float maxFallSpeedYellow = -3f;
	public Material[] playerMats;
	public GameObject grapplePrefab;
	public GameObject coinText;
	public GameObject jumpParticlePrefab;

	public bool ________________;

	public SpriteRenderer spRendMain, spRend1, spRend2, spRend3;
	public TrailRenderer trail1, trail2, trail3;
	Rigidbody rigid;
	public SphereColor _playerColor = SphereColor.red;
	public int currentRoomNumber = 1;
	public Vector3 currentSpawnPoint;

	public bool grounded = true;
	public bool walledLeft = false;
	public bool walledRight = false;
	public float jumping = 0f;
	public float pointAngle = 0f;
	LayerMask[] groundLayerMask;
	SpriteRenderer hookSprite;
	public GameObject playerHook;
	public bool allowsGreen = false;
	public bool allowsBlue = false;
	public bool allowsYellow = false;
	public bool canGrapple = true;
	public bool grappling = false;
	public bool _grappled = false;
	public Grapple hookObj;

	public int coinCount = 0;

	RigidbodyConstraints normal, frozen;

	void Start () {
		// Get components
		instance = this;
		coinCount = 0;
		coinText.GetComponent<Text> ().text = "" + coinCount;
		spRendMain = GameObject.Find ("Player").transform.Find ("SpriteMain").GetComponent<SpriteRenderer> ();
		spRend1 = GameObject.Find ("Player").transform.Find ("Sprite1").GetComponent<SpriteRenderer> ();
		spRend2 = GameObject.Find ("Player").transform.Find ("Sprite2").GetComponent<SpriteRenderer> ();
		spRend3 = GameObject.Find ("Player").transform.Find ("Sprite3").GetComponent<SpriteRenderer> ();
		trail1 = GameObject.Find ("Player").transform.Find ("Sprite1").GetComponent<TrailRenderer> ();
		trail2 = GameObject.Find ("Player").transform.Find ("Sprite2").GetComponent<TrailRenderer> ();
		trail3 = GameObject.Find ("Player").transform.Find ("Sprite3").GetComponent<TrailRenderer> ();
		rigid = GetComponent<Rigidbody> ();
		playerHook = GameObject.Find ("HookedIndicator");
		hookSprite = playerHook.transform.Find ("Sprite").GetComponent<SpriteRenderer> ();

		groundLayerMask = new LayerMask[4];
		groundLayerMask [0] = LayerMask.GetMask ("Object_Default", "Object_Red");
		groundLayerMask [1] = LayerMask.GetMask ("Object_Default", "Object_Blue");
		groundLayerMask [2] = LayerMask.GetMask ("Object_Default", "Object_Green");
		groundLayerMask [3] = LayerMask.GetMask ("Object_Default", "Object_Yellow");

		normal = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
		frozen = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

		currentSpawnPoint = transform.position;

	}

	void Update () {
		if (GameObject.Find ("GrapplingHook(Clone)") == null) {
			grappled = false;
			grappling = false;
		}

		if (grappled)
			return;
		
		if (Input.GetButtonDown(XInput.XboxLB)) {
			//fell, respawn at currentSpawnPoint
			transform.position = currentSpawnPoint;
		}
		// Update colors
		if (Input.GetButtonDown (XInput.XboxA) && allowsGreen && !Utility.instance.paused) playerColor = SphereColor.green;
		if (Input.GetButtonDown (XInput.XboxB) && !Utility.instance.paused) playerColor = SphereColor.red;
		if (Input.GetButtonDown (XInput.XboxX) && allowsBlue && !Utility.instance.paused) playerColor = SphereColor.blue;
		if (Input.GetButtonDown (XInput.XboxY) && allowsYellow && !Utility.instance.paused) playerColor = SphereColor.yellow;

		//////////Hook Indicator////////// 
		// Get indicator angle and grapple
		if ((Mathf.Abs(Input.GetAxis (XInput.XboxRStickX)) > 0.05f || Mathf.Abs(Input.GetAxis (XInput.XboxRStickY)) > 0.05f) && playerColor == SphereColor.green) {
			pointAngle = 180f / Mathf.PI * Mathf.Atan2(Input.GetAxis(XInput.XboxRStickX), Input.GetAxis(XInput.XboxRStickY)) - 90;
			hookSprite.enabled = true;

			if (canGrapple && !grappling && XInput.x.LTDown ()) {
				grappling = true;
				canGrapple = false;
				Instantiate (grapplePrefab);
			}
		} else {
			hookSprite.enabled = false;
		}

		if (!XInput.x.LTDown () && !grappling && playerColor == SphereColor.green) {
			canGrapple = true;
		} else {
			canGrapple = false;
		}

		// Rotate indicator
		playerHook.transform.eulerAngles = new Vector3 (0f, 0f, pointAngle);
	}
	
	void FixedUpdate () {
		if (grappled)
			return;
		
		// Update grounded
		grounded = Physics.Raycast (transform.position, Vector3.down, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledLeft = Physics.Raycast (transform.position, Vector3.left, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);
		walledRight = Physics.Raycast (transform.position, Vector3.right, GetComponent<SphereCollider> ().radius * 1.1f, groundLayerMask [(int)playerColor]);

		if (grounded) {
			jumping = 0;
		}

		//////////Player Movement//////////
		Vector3 moveDir = new Vector3 (Input.GetAxis (XInput.XboxLStickX), 0, 0);
//		if (!grounded) {
//			rigid.drag = 0;
//		} else {
//			rigid.drag = Mathf.Lerp (maxDrag, 0, moveDir.magnitude);
//		}
		float forceApp = 0;
		if ((rigid.velocity.x > 0 && moveDir.x < 0) || (rigid.velocity.x < 0 && moveDir.x > 0)) {
			forceApp = 1;
		} else if ((walledLeft && moveDir.x < 0) || (walledRight && moveDir.x > 0)) {
			forceApp = 0;
		} else {
			forceApp = Mathf.Clamp ((maxSpeed - Mathf.Abs(GetComponent<Rigidbody> ().velocity.x)) / maxSpeed, 0, 1);
		}
		rigid.AddForce (moveDir * forceApp * moveForce);

		//////////Player jumping//////////
		// Normal jump
		if (XInput.x.RTDown() && jumping == 0) {
			jumping = 1;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			rigid.velocity = vec;

			if (grounded) {
				// Start Particle Effect
				GameObject pe = Instantiate(jumpParticlePrefab, new Vector3(transform.position.x, transform.position.y  - GetComponent<SphereCollider>().radius, transform.position.z), Quaternion.identity) as GameObject;
				Destroy (pe, 0.5f);
			}
		}

		// Double jump if blue
		if (!XInput.x.RTDown () && jumping == 1) {
			jumping = 2;
		}
		if (XInput.x.RTDown () && jumping == 2 && !grounded && !walledLeft && !walledRight && playerColor == SphereColor.blue) {
			jumping = 3;
			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed / 1.7f;
			rigid.velocity = vec;
		}

		// Glide if yellow
		if (XInput.x.RTDown () && jumping > 0 && playerColor == SphereColor.yellow && rigid.velocity.y < maxFallSpeedYellow) {
			Vector3 vec = rigid.velocity;
			vec.y = maxFallSpeedYellow;
			rigid.velocity = vec;
		}

		// Wall jump left
		if (XInput.x.RTDown () && walledLeft) {
			jumping = 1;

			GameObject pe = Instantiate(jumpParticlePrefab, new Vector3(transform.position.x - GetComponent<SphereCollider>().radius, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			Destroy (pe, 0.5f);

			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed / 1.155f;
			if (vec.x < jumpSpeed / 2f) {
				vec.x = jumpSpeed / 2f;
			} else {
				vec.x = -vec.x;
			}
			rigid.velocity = vec;
		}

		// Wall jump right
		if (XInput.x.RTDown () && walledRight) {
			jumping = 1;

			GameObject pe = Instantiate(jumpParticlePrefab, new Vector3(transform.position.x + GetComponent<SphereCollider>().radius, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			Destroy (pe, 0.5f);

			Vector3 vec = rigid.velocity;
			vec.y = jumpSpeed;
			if (vec.x > -jumpSpeed / 2f) {
				vec.x = -jumpSpeed / 2f;
			} else {
				vec.x = -vec.x;
			}
			rigid.velocity = vec;
		}

		// Stop jump
		if (!XInput.x.RTDown () && jumping > 0 && rigid.velocity.y > 0) {
			Vector3 vec = rigid.velocity;
			vec.y = vec.y / 3;
			rigid.velocity = vec;
		}
	}

	public bool grappled {
		get { return _grappled; }
		set { if (value == _grappled)
				return;
			if (value) {
				rigid.constraints = frozen;
				rigid.velocity = Vector3.zero;
			} else {
				rigid.constraints = normal;
			}
			_grappled = value;
		}
	}

	public SphereColor playerColor {
		get { return _playerColor; }
		set { 	
			if (_playerColor == value)
				return;
			else {
				Color temp = Color.black;
				switch (value) {
				case SphereColor.red:
					temp = new Color (1f, 0f, 0f);
					if (spRend1.color == temp || spRend1.color == Color.white) {
						spRend1.color = spRendMain.color;
						trail1.material.SetColor ("_TintColor", spRendMain.color);
					} else if (spRend2.color == temp || spRend2.color == Color.white) {
						spRend2.color = spRendMain.color;
						trail2.material.SetColor ("_TintColor", spRendMain.color);
					} else {
						spRend3.color = spRendMain.color;
						trail3.material.SetColor ("_TintColor", spRendMain.color);
					}
					spRendMain.color = temp;
					gameObject.layer = 8;
					break;
				case SphereColor.blue:
					temp = new Color (0f, 0f, 1f);
					if (spRend1.color == temp || spRend1.color == Color.white) {
						spRend1.color = spRendMain.color;
						trail1.material.SetColor ("_TintColor", spRendMain.color);
					} else if (spRend2.color == temp || spRend2.color == Color.white) {
						spRend2.color = spRendMain.color;
						trail2.material.SetColor ("_TintColor", spRendMain.color);
					} else {
						spRend3.color = spRendMain.color;
						trail3.material.SetColor ("_TintColor", spRendMain.color);
					}
					spRendMain.color = temp;
					gameObject.layer = 9;
					break;
				case SphereColor.green:
					temp = new Color (0f, 125f/255f, 0f);
					if (spRend1.color == temp || spRend1.color == Color.white) {
						spRend1.color = spRendMain.color;
						trail1.material.SetColor ("_TintColor", spRendMain.color);
					} else if (spRend2.color == temp || spRend2.color == Color.white) {
						spRend2.color = spRendMain.color;
						trail2.material.SetColor ("_TintColor", spRendMain.color);
					} else {
						spRend3.color = spRendMain.color;
						trail3.material.SetColor ("_TintColor", spRendMain.color);
					}
					spRendMain.color = temp;
					gameObject.layer = 10;
					break;
				case SphereColor.yellow:
					temp = new Color (1f, 1f, 0f);
					if (spRend1.color == temp || spRend1.color == Color.white) {
						spRend1.color = spRendMain.color;
						trail1.material.SetColor ("_TintColor", spRendMain.color);
					} else if (spRend2.color == temp || spRend2.color == Color.white) {
						spRend2.color = spRendMain.color;
						trail2.material.SetColor ("_TintColor", spRendMain.color);
					} else {
						spRend3.color = spRendMain.color;
						trail3.material.SetColor ("_TintColor", spRendMain.color);
					}
					spRendMain.color = temp;
					gameObject.layer = 11;
					break;
				}
				transform.FindChild("Light").GetComponent<Light> ().color = temp;
				_playerColor = value;
			}
		}
	}

	public void resetPlayerToCurrentSpawn() {
		transform.position = currentSpawnPoint;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Coin") {
			//add coin to player
		} else if (coll.gameObject.tag == "Checkpoint") {
			currentSpawnPoint = coll.transform.position;
		} else if (coll.gameObject.tag == "Ground") {
			if (grappled) {
			grappled = false;
			hookObj.DestroyAll ();
			}
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Ground") {
			// Start Particle Effect
			GameObject pe = Instantiate(jumpParticlePrefab, coll.contacts[0].point, Quaternion.identity) as GameObject;
			Destroy (pe, 0.5f);
		}
	}
}