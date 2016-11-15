using UnityEngine;
using System.Collections;

public class Gen_Running_Main_Menu : MonoBehaviour {

    public float runningSpeed;
    public Transform tele;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.right * runningSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        transform.position = tele.position;
    }
}
