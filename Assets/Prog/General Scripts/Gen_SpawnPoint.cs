using UnityEngine;
using System.Collections;

/*
// Handles spawning for the characters
*/
public class Gen_SpawnPoint : MonoBehaviour
{
    public bool IsValid = true;
    private int _PlayerCount = 0;
    // Use this for initialization
    void Start()
    {
        if (_PlayerCount == 0)
        {
            IsValid = true;
        }
        else
        {
            IsValid = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_PlayerCount == 0)
        {
            IsValid = true;
        }
        else
        {
            IsValid = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ++_PlayerCount;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            --_PlayerCount;
        }
    }
}
