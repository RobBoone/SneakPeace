using UnityEngine;
using System.Collections;

/*
// Makes object look to the camera (fake UI)
*/
public class Gen_LookAtCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

       transform.LookAt(Camera.main.transform);

	}
}
