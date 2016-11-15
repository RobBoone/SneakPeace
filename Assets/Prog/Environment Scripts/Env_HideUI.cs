using UnityEngine;
using System.Collections;


/*
// Hides Ui when it is obstructive (a.e. in front of character)
*/ 
public class Env_HideUI : MonoBehaviour {

    public CanvasRenderer _Image;
    private bool SomeoneInCanvasZone = false;
    private float AlphaValue = 1.0f;
	
	// Update is called once per frame
	void Update ()
    {

	    if(SomeoneInCanvasZone)
        {
            if(AlphaValue>=0.3f)
            AlphaValue -= Time.deltaTime*4;

                
        }
        else
        {
            if (AlphaValue <= 1.0f)
                AlphaValue += Time.deltaTime*2;
        }

        _Image.SetAlpha(AlphaValue);
        var allChildren = _Image.gameObject.GetComponentsInChildren<CanvasRenderer>();
        foreach (CanvasRenderer child in allChildren)
        {
            child.SetAlpha(AlphaValue);
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            SomeoneInCanvasZone = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            SomeoneInCanvasZone = false;
        }
    }
}
