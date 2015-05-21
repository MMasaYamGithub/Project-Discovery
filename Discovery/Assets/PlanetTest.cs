using UnityEngine;
using System.Collections;

public class PlanetTest : MonoBehaviour {

	public float rotationSpeed = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, rotationSpeed, 0);
	}
}
