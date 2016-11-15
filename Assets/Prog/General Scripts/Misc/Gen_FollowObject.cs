using UnityEngine;
using System.Collections;

public class Gen_FollowObject : MonoBehaviour {

    public Transform FollowObject;

    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;

	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(FollowObject.position.x + OffsetX, FollowObject.position.y + OffsetY, FollowObject.position.z + OffsetZ);
	}
}
