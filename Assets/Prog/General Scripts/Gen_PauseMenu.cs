#define WindowsBuild
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
// Handles ingame pauze menu
// Is player based. Cannot pauze when you are not controlling a player
// Only player who pauzes can controll menu
*/
public class Gen_PauseMenu : MonoBehaviour {
    private GameObject[] ButtonArray = new GameObject[3];
    public GameObject _ButtonRestart;
    public GameObject _ButtonMainMenu;
    public GameObject _ButtonQuit;
    public Env_Pause _PauseScript;
    private int pausedPlayer;

    public AudioSource SelectButton;
    public AudioSource ScrollButton;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    private int LevelSelect;

    float _LeftStickY = 0;
    bool flag = true;

    int _Counter = 0;

    void Start()
    {
#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(1);
#endif
        // LevelSelect = 0;
        //_Controls.enabled = false;
        ButtonArray[0] = _ButtonRestart;
        ButtonArray[1] = _ButtonMainMenu;
        ButtonArray[2] = _ButtonQuit;
    }


    void Update()
    {

        pausedPlayer = _PauseScript._PausedPlayer;
        //Debug.Log(pausedPlayer);
#if WindowsBuild
       if(pausedPlayer==0)
       _Controller = Gen_ControllerManager.Instance.GetController(1);
       else
        _Controller = Gen_ControllerManager.Instance.GetController(pausedPlayer);
       // Debug.Log(_Controller.Index);
        if (_Controller.IsConnected)
        {
            _LeftStickY = _Controller.GetStick_L().Y;
        }
#else
        if (Time.timeScale == 0)
            {
                _LeftStickY = Input.GetAxis("LeftStickY" + pausedPlayer.ToString());
           
            }
#endif

            if ((_LeftStickY == 0.0f))
            {
                flag = true;
            }

            if (_LeftStickY >= .75f && flag)
            {
                flag = false;
                Cycle(false);
            }
            if (_LeftStickY <= -.75f && flag)
            {
                flag = false;
                Cycle(true);
            }

        //Debug.Log(_LeftStickY);
        //Debug.Log(_Counter);
#if WindowsBuild
        if (Time.timeScale == 0)
        {
            if (_Controller.IsConnected)
            {
                ButtonArray[_Counter].GetComponent<Button>().Select();

                if (_Controller.GetButtonDown("A"))
                {
                    switch (_Counter)
                    {
                        case 0:
                            Restart();
                            break;
                        case 1:
                            MainMenu();
                            break;
                        case 2:
                            Quit();
                            break;
                    }
                }
            }
        }
        else if (Time.timeScale == 1.0f)
            _Counter = 0;
#else
        if (Time.timeScale == 0)
        {
            //if (ButtonArray[_Counter].GetComponent<Button>().IsActive())
            //Debug.Log(_Counter);
            //Debug.Log(ButtonArray[_Counter].GetComponent<Button>().IsActive());

            ButtonArray[_Counter].GetComponent<Button>().Select();

            //Debug.Log(_Counter);
            if (Input.GetButtonDown("Ready" + pausedPlayer.ToString()))
            {
                switch (_Counter)
                {
                    case 0:
                        Restart();
                        break;
                    case 1:
                        MainMenu();
                        break;
                    case 2:
                        Quit();
                        break;
                }
            }
        }
        else if (Time.timeScale == 1.0f)
            _Counter = 0;


#endif




        //if (_PauseScript._Pause)
        //_Counter = 0;

    }


    void Cycle(bool direction)
    {
        if (direction)
        {
            if (_Counter < ButtonArray.Length - 1)
            {
                _Counter += 1;

                ScrollButton.Play();

            }
        }
        else
        {
            if (_Counter > 0)
            {
                _Counter -= 1;

                ScrollButton.Play();

            }

        }


    }


    public void MainMenu()
    {

        SelectButton.Play();

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Restart()
    {

        SelectButton.Play();

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        Time.timeScale = 1;

    }

    public void Quit()
    {
        SelectButton.Play();

        Application.Quit();
        Time.timeScale = 1;
    }

}
