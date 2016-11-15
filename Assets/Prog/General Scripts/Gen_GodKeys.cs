using UnityEngine;
using System.Collections;

/*
// Adds cheat key's to the game to allow for demonstration and debugging
*/
public class Gen_GodKeys : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKey("1")) 
        {
            GameObject.Find("Player1").GetComponent<Char_Manager>().SetDefault(WeaponType.Laser);
        }

        if (Input.GetKey("2"))
        {
            GameObject.Find("Player1").GetComponent<Char_Manager>().SetDefault(WeaponType.Grenades);
        }


        if (Input.GetKey("3"))
        {
            GameObject.Find("Player1").GetComponent<Char_Manager>().SetDefault(WeaponType.Mines);
        }

        if (Input.GetKey("4"))
        {
            GameObject.Find("Player1").GetComponent<Char_Manager>().SetDefault(WeaponType.Shotgun);
        }

        if (Input.GetKeyDown("5"))
        {
            GameObject.Find("Player1").GetComponent<Char_Manager>().Shield(3, true);
        }


        if (Input.GetKeyDown("6"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown("7"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }

    }
}
