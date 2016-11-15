using UnityEngine;
using System.Collections.Generic;

/*
// Spawns pickup at predefined places
*/
public class Gen_Spawn_Pickup : MonoBehaviour {

    public GameObject[] SpawnArray = new GameObject[5];
    public float RandomLowSpawnTime = 6.0f;
    public float RandomHighSpawnTime = 13.0f;

    public int PickupsAvailable;
    public float RespawnTime;

    public int MaxSpawns;

    //public GameObject PickUpPrefab;
    //public DictionaryBase avaiablespaws=new DictionaryBase;
    public Dictionary<GameObject, bool> SpawnsAvailable= new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, float> SpawnsTimes=new Dictionary<GameObject, float>();

    public GameObject PickUpPrefab;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < SpawnArray.Length; i++)
        {
            SpawnsAvailable.Add(SpawnArray[i], true);
            
            SpawnsTimes.Add(SpawnArray[i], Random.Range(RandomLowSpawnTime, RandomHighSpawnTime));
            
        }
       

        for(int i=0;i<MaxSpawns;i++)
        {
            SpawnPickUp(SpawnArray[i]);
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    { 


        if (PickupsAvailable < MaxSpawns)
        {
            for (int i = 0; i < SpawnArray.Length; i++)
            {
                SpawnsTimes[SpawnArray[i]] -= Time.deltaTime;

                if(SpawnsAvailable[SpawnArray[i]]&& SpawnsTimes[SpawnArray[i]]<0)
                {
                    SpawnPickUp(SpawnArray[i]);
                }
            }
        }
    }

    public void PickedUp(GameObject SpawnPos)
    {
        SpawnsAvailable[SpawnPos] = true;
        PickupsAvailable -= 1;
        if (PickupsAvailable < MaxSpawns)
        {
            
            SpawnsTimes[SpawnPos] = Random.Range(RandomLowSpawnTime, RandomHighSpawnTime);
        }
    }

    void SpawnPickUp(GameObject spawnPos)
    {
        if (SpawnsAvailable[spawnPos])
        {
            GameObject instance= Instantiate(PickUpPrefab, spawnPos.transform.position, Quaternion.identity)as GameObject;
            instance.GetComponent<Env_Pickup>().SetSpawnPoint(spawnPos);
            PickupsAvailable += 1;
            SpawnsAvailable[spawnPos] = false;
        }
    }
}
