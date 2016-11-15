using UnityEngine;
using System.Collections;

public class Gen_DoNotRot : MonoBehaviour {

    private Vector3 rotation;

	// Use this for initialization
	void Start () {
        rotation = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 tempRot = transform.rotation.eulerAngles;

        tempRot.x = rotation.x;
        tempRot.z = rotation.z;

        transform.rotation = Quaternion.Euler(tempRot);
	}
}
