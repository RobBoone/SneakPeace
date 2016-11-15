using UnityEngine;
using System.Collections;

/*
// Handles behaviour of the mines
*/
public class Env_Mine : MonoBehaviour {

    public GameObject playerToScore;
    public GameObject Grenade;
    public Color playerColor;

    public float ArmTime = 0.5f;

    float liveTime = 0;

    /*
    //Mines can be spawned by the player.
    // When mines explode they create a zero cooldown grendade
    // Thus the mines are dependand on the grenades to work.
    */
	
	// Update is called once per frame
	void Update () {

        if(ArmTime >= 0)
            ArmTime -= Time.deltaTime;

        liveTime += Time.deltaTime;

        if (liveTime < 1)
        {
            Color c = transform.FindChild("Base").GetComponent<Renderer>().material.color;
            c.a = 1 - liveTime / 1;

            transform.FindChild("Base").GetComponent<Renderer>().material.color = c;

            transform.FindChild("Indicator").GetComponent<Renderer>().enabled = false;
        }
        else
        {
            transform.FindChild("Base").GetComponent<Renderer>().enabled = false;

            float t = Mathf.PingPong(liveTime, 1.8f);

            transform.FindChild("Indicator").GetComponent<Renderer>().enabled = t > 1 ? true : false;
        }

    }


    //Mine explodes when someone is too near and the mine is armed
    void OnTriggerEnter(Collider other)
    {

        if (ArmTime > 0)
            return;

       
        //Explode
        var t = Instantiate(Grenade, transform.position, Quaternion.identity) as GameObject;
        t.GetComponent<Env_GrenadeScript>().playerColor = playerColor;
        t.GetComponent<Env_GrenadeScript>().UsedAsMine = true;
        t.transform.FindChild("TempGrenadeIndicator").GetComponent<Env_DetonationScript>().playerToScore = playerToScore;
        t.GetComponent<AudioSource>().Play();
        Destroy(this.gameObject);

    }
}
