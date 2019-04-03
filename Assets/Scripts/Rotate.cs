using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float rotation_speed;
	public Vector3 rotation_vector;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotation_vector * Time.deltaTime * rotation_speed);
	}
}
