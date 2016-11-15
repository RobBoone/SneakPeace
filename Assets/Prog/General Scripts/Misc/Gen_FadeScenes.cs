using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Gen_FadeScenes : MonoBehaviour {

    public Texture2D FadeOutTexture;
    public float FadeSpeed = 0.8f;

    private int _DrawDepth = -1000;
    private float _Alpha = 1.0f;
    private int _FadeDir = -1;

    void OnGui()
    {
        _Alpha += _FadeDir * FadeSpeed * Time.deltaTime;
        _Alpha = Mathf.Clamp01(_Alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _Alpha);
        GUI.depth = _DrawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        _FadeDir = direction;
        return (FadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
}
