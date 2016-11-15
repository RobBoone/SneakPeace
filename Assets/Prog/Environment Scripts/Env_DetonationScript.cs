using UnityEngine;
using System.Collections;

/*
// Script used for detonating the grenades (and with that the mines)
*/
public class Env_DetonationScript : MonoBehaviour {

    private bool exploded=false;
    public GameObject playerToScore;

	void Update () {
        exploded = this.transform.parent.GetComponent<Env_GrenadeScript>().Exploded;
        this.transform.position = this.transform.parent.position;
	}

    //Everyone who is inside or runes inside the detonation area gets hit.
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player") 
            && 
            exploded 
            && 
            other.gameObject.GetComponent<Char_Manager>()._LifeState==LifeState.Alive
            &&
            !other.gameObject.GetComponent<Char_Manager>()._Shielded)
        {

            playerToScore.GetComponent<Char_Manager>().HitSomeone(other.gameObject);
            
            other.gameObject.GetComponent<Char_Manager>().GotHit(transform, playerToScore);
        }
    }


}



