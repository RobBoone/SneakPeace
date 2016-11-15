#define WindowsBuild

#if WindowsBuild
using UnityEngine;
using System.Collections;


public class Gen_RefreshController : MonoBehaviour {


	// Update is called once per frame
	void Update ()
    {
        Gen_ControllerManager.Instance.Refresh();
	}
}
#endif
