using UnityEngine;
using System.Collections;

/*
// destroys this object _timer after it was created :D
*/
public class Gen_Self_Destroy : MonoBehaviour {

    public float _Timer = -36;//Leave at -36 to not self destroy

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(_Timer != -36)
        {
            _Timer -= Time.deltaTime;

            if(_Timer <=0)
            {
                Destroy(this.gameObject);
            }
        }

	}
}
