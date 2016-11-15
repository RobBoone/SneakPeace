using UnityEngine;
using System.Collections;

/*
// Handles grenade behaviour.
*/
public class Env_GrenadeScript : MonoBehaviour {

    public bool Exploded = false;
    public float countDown = 0.5f;

    public GameObject Explosion;
    public Color playerColor;

    public bool UsedAsMine = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Exploded)
        {
            countDown -= Time.deltaTime;

            if (countDown <= 0)
            {
                Exploded = false;
                countDown = 0.5f;
                Destroy(this.gameObject);
            }

        }
      
	}

    //When it hits something on its flight path, it explodes
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Level"))
        {

            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            var t = Instantiate(Explosion, transform.position, Quaternion.identity) as GameObject;
            t.transform.FindChild("SE").GetComponent<ParticleSystem>().startColor = playerColor;
            t.transform.FindChild("LE").GetComponent<Light>().color = playerColor;
            t.GetComponent<AudioSource>().Play();

            Exploded = true;
        }
    }
}
