using UnityEngine;
using System.Collections;

public class Gen_SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Countdown");
	}
	
	// Update is called once per frame
	private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4);

        float fadeTime = GetComponent<Gen_FadeScenes>().BeginFade(1);
        //yield return new WaitForSeconds(fadeTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
