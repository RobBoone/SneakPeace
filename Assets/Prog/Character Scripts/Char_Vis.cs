using UnityEngine;
using System.Collections;

public class Char_Vis : MonoBehaviour {

    /*
    //Handles visibility and invisiblilty of characters and weapons
    */

    Color TargetColor;
    float VisTimer = -36;

    //times how long you can idle until becoming visible
    float IdleTimer = -36;
    public float IdleTime = 4;

    public GameObject[] OtherPlayers;
    public GameObject PredatorCamo;

    // Use this for initialization
    void Start () {
        IdleTimer = IdleTime;
    }
	
	// Update is called once per frame
	public void VisibleUpdate() {

        /*
        //Set color of the objects to fade to the target
        */
        this.transform.FindChild("Model").FindChild("Render").gameObject.GetComponent<Renderer>().material.color =
            Color.Lerp(this.transform.FindChild("Model").FindChild("Render").gameObject.GetComponent<Renderer>().material.color,
                TargetColor, 0.5f);

        //Time how long you can remain visible until becoming unvisible
        if (VisTimer > 0 && VisTimer != -36)
        {
            VisTimer -= Time.deltaTime;
            if(VisTimer <= 0)
            {
                MakeInvis();
                VisTimer = -36;
            }
        }

        //All ze functies
        IsNear();
        IsIdle();

        //Make the guns invisible
        GetComponent<Char_Manager>()._Sniper.GetComponent<Renderer>().material.color = this.transform.FindChild("Model").FindChild("Render").GetComponent<Renderer>().material.color;
        GetComponent<Char_Manager>()._Shotgun.GetComponent<Renderer>().material.color = this.transform.FindChild("Model").FindChild("Render").GetComponent<Renderer>().material.color;

    }

    //Check if close to any of the other players
    public void IsNear()
    {
        
        for (int i = 0; i < OtherPlayers.Length; i++)
        {
            var distance = Vector3.Distance(OtherPlayers[i].transform.position, transform.position);

            //if close enough and player isn't active make visible
            if (distance < 10 && OtherPlayers[i].activeInHierarchy)
            {
                SetTargetOpacity(1 - distance / 10);
                PredatorCamo.SetActive(false);
            }
        }
    }

    public void IsIdle()
    {

        //Count down
        if (IdleTimer != -36)
        {
            IdleTimer -= Time.deltaTime;

            if(IdleTimer <= 0)
            {
                SetTargetOpacity(Mathf.PingPong(Time.time * 2, 1.0f));
                setVisTimer(0.4f);
                IdleTimer = 0;
            }
        }

    }

    public void Moved()
    {
        IdleTimer = IdleTime;
    }

    public void MakeVis()
    {
        TargetColor = new Color(1, 1, 1, 1);
        PredatorCamo.SetActive(false);
    }

    public void MakeVisDeath()
    {
        TargetColor = new Color(1, 0, 0, 1);
        PredatorCamo.SetActive(false);
    }

    public void MakeInvis()
    {
        TargetColor = new Color(1, 1, 1, 0);
        PredatorCamo.SetActive(true);
    }

    public void SetTargetOpacity(float opacity)
    {
        if (GetComponent<Char_Manager>()._LifeState != LifeState.Dying)
        {
            TargetColor = new Color(1, 1, 1, opacity);
            setVisTimer(1);
        }
    }

    public void setVisTimer(float time)
    {
        VisTimer = time;
    }
}
