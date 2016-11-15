using UnityEngine;
using System.Collections;

/*
//  Handles the characters dying being death and respawning after given times
*/
public class Char_Death : MonoBehaviour {

    /*
    // VARIABLES
    */
    float DyingTimer = -36;
    float DeathTimer = -36;

    public float dyingTime = 0; //How long a character is dying
    public float deathTime = 0; //How long a character stays dead
    
    Char_Manager ManagerScript; //Link to the manager
    
	void Start () {
        ManagerScript = GetComponent<Char_Manager>();
    }
	
	void Update () {
	    
        //Dying timer - While dying character moves up
        if(DyingTimer > 0 && DyingTimer != -36)
        {
            DyingTimer -= Time.deltaTime;

            //Animate up
            transform.Translate(new Vector3(0, 0.05f, 0));

            if (DyingTimer <= 0)
            {
                Dying(LifeState.Death);
            }
        }

        //Death timer
        if (DeathTimer > 0 && DeathTimer != -36)
        {
            DeathTimer -= Time.deltaTime;
            if (DeathTimer <= 0)
            {
                ManagerScript._LifeState = LifeState.Respawning;
                ManagerScript.SpawnCharacter();
            }
        }

    }


    /*
    // Cycles trough the lifestate cycle
    // Alive -> Dying -> Death -> Respawning
    // Also calls the manager script dying and death so other scripts can get called for on death and dying events.
    */
    public void Dying(LifeState state)
    {
        switch (state)
        {
            case LifeState.Dying:
                ManagerScript.Dying();
                DyingTimer = dyingTime;
                break;
            case LifeState.Death:
                ManagerScript.Death();
                DeathTimer = deathTime;
                break;
        }
    }
}
