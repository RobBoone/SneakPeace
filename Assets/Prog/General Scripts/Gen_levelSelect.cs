#define WindowsBuild
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
// Handles level selection
*/
public class Gen_levelSelect : MonoBehaviour {

    public Button Jungle;
    public Button SwanLake;

    public AudioSource ButtonSelect;
    public AudioSource ButtonScroll;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    float _LeftStickX = 0;
    bool _Moved = false;
    bool _JungleSelected = true;

    // Use this for initialization
    void Start () {
        Jungle = Jungle.GetComponent<Button>();
        SwanLake = SwanLake.GetComponent<Button>();
        Jungle.Select();
    }
	
	// Update is called once per frame
	void Update ()
    {

#if WindowsBuild
       _Controller = Gen_ControllerManager.Instance.GetController(1);
        if (_Controller.IsConnected)
        {
            _LeftStickX = _Controller.GetStick_L().X;
        }
#else
         _LeftStickX = Input.GetAxis("LeftStickX1");
#endif
        if(_LeftStickX == 0.0f && _Moved)
        {
            _Moved = false;
        }

        if ((_LeftStickX > 0.5f || _LeftStickX < -0.5f) && !_Moved)
        {
            _Moved = true;
            ChangeSelected();
        }

#if WindowsBuild
        if (_Controller.IsConnected)
        {
           if (_Controller.GetButtonDown("A"))
           {
                ClickSelected();
           }
        }
#else
        if (Input.GetButtonDown("Ready1"))
        {
            ClickSelected();
        }
#endif


        }

    void ChangeSelected()
    {
        if(_JungleSelected)
        {
            _JungleSelected = false;
            SwanLake.Select();

            ButtonScroll.Play();
        }
        else
        {
            _JungleSelected = true;
            Jungle.Select();

            ButtonScroll.Play();
        }
    }

    void ClickSelected()
    {
        if (_JungleSelected)
        {
            ButtonSelect.Play();
            Jungle.onClick.Invoke();
        }
        else
        {
            ButtonSelect.Play();
            SwanLake.onClick.Invoke();
        }
    }

    public void JungleClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void SwanLakeClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}
