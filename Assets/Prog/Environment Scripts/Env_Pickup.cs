using UnityEngine;
using System.Collections;

/*
// Handles behaviour of the pickups on the map
*/
public class Env_Pickup : MonoBehaviour {

    GameObject _spawnPoint;
    public Gen_Spawn_Pickup _spawnerScript;
    public GameObject PickupPart;
    
	// Use this for initialization
	void Start () {
        _spawnerScript = GameObject.Find("SpawnPickups").GetComponent<Gen_Spawn_Pickup>();
    }

    void OnTriggerEnter(Collider e)
    {
        if (e.CompareTag("Player"))
        {
            _spawnerScript.PickedUp(_spawnPoint);
            e.GetComponent<Char_Manager>().PickUp();

            var t = Instantiate(PickupPart, 
                    new Vector3(e.transform.position.x, e.transform.position.y - 2f, e.transform.position.z),
                    Quaternion.Euler(new Vector3(-90,0,0))) as GameObject;

            t.transform.SetParent(e.transform);
            Destroy(this.gameObject);
        }
    }

    public void SetSpawnPoint(GameObject spawnpoint)
    {
        _spawnPoint = spawnpoint;
    }


}
