using UnityEngine;
using System.Collections;

public class NaturalSatellite : MonoBehaviour {
	
	public Transform target;
	public float RotationSpeed = 100f;
	public float OrbitDegrees = 1f;

	void Update () {
		transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
		transform.position = RotatePointAroundPivot(transform.position, transform.parent.position, Quaternion.Euler(0, OrbitDegrees * Time.deltaTime, 0));
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot) + pivot;
	}
}
