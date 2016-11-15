using UnityEngine;


/*
// Handles walkable area (spawns grass or footsteps when walking over theses)
*/
public class Env_Triggers : MonoBehaviour
{
    public Material st;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var em = other.transform.FindChild("Grass").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            var em = other.transform.FindChild("Grass").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
        }
    }
}
