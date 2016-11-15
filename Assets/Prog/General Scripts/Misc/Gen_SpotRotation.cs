using UnityEngine;
using System.Collections;

public class Gen_SpotRotation : MonoBehaviour {

    private float _RotationSpeedY;

    void Start()
    {
        _RotationSpeedY = Random.Range(2.0f, 6.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        //transform.localRotation = Quaternion.Euler(Mathf.PingPong(Time.time * _RotationSpeedX, _AngleLimitX*2) - _AngleLimitX, Mathf.PingPong(Time.time * _RotationSpeedY, _AngleLimitY*2) - _AngleLimitY, Mathf.PingPong(Time.time * _RotationSpeedZ, _AngleLimitZ*2) - _AngleLimitZ);
        transform.Rotate(0, 0, Time.deltaTime * _RotationSpeedY);

    }
}
