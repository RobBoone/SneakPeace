#define WindowsBuild
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
// Handles pauzing the game
*/
public class Env_Pause : MonoBehaviour {


    public bool _Pause = false;
    public Canvas PauseCanvas;
    public int _PausedPlayer;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif
    public void Pause(int player, bool Keyboard)
    {
        playerInput(player);


#if WindowsBuild
        if (_Controller.IsConnected)
        {
            if (_Controller.GetButtonDown("Start"))
            {
                _PausedPlayer = _Controller.Index+1;
                Debug.Log(_PausedPlayer);
                if (Time.timeScale == 1.0f)
                {
                    Time.timeScale = 0.0f;
                    PauseCanvas.transform.FindChild("Restart").gameObject.SetActive(true);
                    PauseCanvas.transform.FindChild("Stop").gameObject.SetActive(true);
                    PauseCanvas.transform.FindChild("MainMenu").gameObject.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1.0f;
                    PauseCanvas.transform.FindChild("Restart").gameObject.SetActive(false);
                    PauseCanvas.transform.FindChild("Stop").gameObject.SetActive(false);
                    PauseCanvas.transform.FindChild("MainMenu").gameObject.SetActive(false);
                }
            }
        }

        //Debug.Log(Time.timeScale);
#else



        if (_Pause)
        {

            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.0f;
                PauseCanvas.transform.FindChild("Restart").gameObject.SetActive(true);
                PauseCanvas.transform.FindChild("Stop").gameObject.SetActive(true);
                PauseCanvas.transform.FindChild("MainMenu").gameObject.SetActive(true);
                
            }
            else
            {
                Time.timeScale = 1.0f;
                PauseCanvas.transform.FindChild("Restart").gameObject.SetActive(false);
                PauseCanvas.transform.FindChild("Stop").gameObject.SetActive(false);
                PauseCanvas.transform.FindChild("MainMenu").gameObject.SetActive(false);
            }
        }



#endif
        //Debug.Log(Time.timeScale);

        //if (_Pause > 0)
        //{
        //    if ()
        //    {
        //    
        //    }
        //}

    }

    void playerInput(int playNum)
    {
#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(playNum);
        
        
#else
        //if (Time.timeScale == 1.0f)
        //{
        _Pause = Input.GetButtonDown("Start" + playNum.ToString());
            if (_Pause)
            _PausedPlayer = playNum;

       // }
       // else
       // {
       //     _Pause = Input.GetButtonDown("Start" + pausedPlayer.ToString());
       // }
       // 
        
#endif
            //Debug.Log(Input.GetButtonDown("Start" + playNum.ToString()));

    }
}
