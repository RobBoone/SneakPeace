using UnityEngine;
using System.Collections;

public class Gen_Rotate : MonoBehaviour {

    public float RotationSpeed=100;
	// Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        this.transform.FindChild("Question").Rotate(0, -2*RotationSpeed * Time.deltaTime, 0);
    }
}
