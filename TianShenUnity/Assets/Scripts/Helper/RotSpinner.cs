using UnityEngine;
using System.Collections;

public class RotSpinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 oldRot = transform.eulerAngles;
		transform.rotation = Quaternion.Euler(oldRot.x, oldRot.y, oldRot.z - 270 * Time.deltaTime);

		//transform.RotateAround(Vector3.forward, 180 * Time.deltaTime);
	}
}
