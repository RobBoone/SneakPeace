using UnityEngine;
using System.Collections;

public class Gen_MainSpotRot : MonoBehaviour {

    private float _RotateSpeed;

    void Start ()
    {
        _RotateSpeed = Random.Range(-10.0f, -15.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(0.0f, 0.0f, _RotateSpeed * Time.deltaTime);
    }
}
