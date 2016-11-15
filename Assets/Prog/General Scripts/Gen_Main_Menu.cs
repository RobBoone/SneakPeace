#define WindowsBuild
using UnityEngine;
using UnityEngine.UI;

/*
// Handels cycling trough menu items and menu selection
*/
public class Gen_Main_Menu : MonoBehaviour
{

    private GameObject[] _ButtonArray = new GameObject[4];
    public GameObject ButtonPlay;
    public GameObject ButtonControls;
    public GameObject ButtonQuit;
    public GameObject ButtonBack;
    public GameObject Title;

    public GameObject Movie1;
    public GameObject Movie2;
    public GameObject Movie3;
    public GameObject Movie4;

    public AudioSource ButtonSound;
    public AudioSource ButtonSelect;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    public GUISkin MenuGUISkin;
    public GameObject _Controls;
    //private int LevelSelect;
  
    float _LeftStickY = 0;
    bool flag = true;

    int _Counter = 0;
    bool _BackZone = false;

    void Start()
    {
#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(1);
#endif
        // LevelSelect = 0;
        //_Controls.enabled = false;
        _ButtonArray[0] = ButtonPlay;
        _ButtonArray[1] = ButtonControls;
        _ButtonArray[2] = ButtonQuit;
        _ButtonArray[3] = ButtonBack;

        if (_ButtonArray[_Counter] != null)
        {
            _ButtonArray[_Counter].GetComponent<Button>().Select();
        }
    }


    void Update()
    {
#if WindowsBuild
        if (_Controller.IsConnected)
        {
            if( !_BackZone)
            _LeftStickY = _Controller.GetStick_L().Y;
        }
#else
        if (!_BackZone)
            _LeftStickY = Input.GetAxis("LeftStickY1");
#endif
        if ((_LeftStickY == 0.0f ))
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
        if (_Controller.IsConnected)
        {
            if (_Controller.GetButtonDown("A"))
            {
                switch (_Counter)
                {
                    case 0:
                        LevelPlaySelect();
                        break;
                    case 1:
                        Controls();
                        break;
                    case 2:
                        Quit();
                        break;
                    case 3:
                        Back();
                        break;
                }
            }
        }
#else


        if (Input.GetButtonDown("Ready1"))
        {
            switch (_Counter)
            {
                case 0:
                    LevelPlaySelect();
                    break;
                case 1:
                    Controls();
                    break;
                case 2:
                    Quit();
                    break;
                case 3:
                    Back();
                    break;
            }
        }


#endif

        //Select buttons
        if (_ButtonArray[_Counter] != null)
        {
            _ButtonArray[_Counter].GetComponent<Button>().Select();
        }
    }


    void Cycle(bool direction)
    {
        if (direction) 
        {
            if (_Counter < _ButtonArray.Length - 2)
            {
                _Counter += 1;

                ButtonSound.Play();

            }

            

        }
        else 
        {
            if (_Counter > 0)
            {
                _Counter -= 1;

                ButtonSound.Play();

            }

           
        }

       
    }


    public void LevelPlaySelect()
    {
        //float fadeTime = GetComponent<Gen_FadeScenes>().BeginFade(1);
        //yield return new WaitForSeconds(fadeTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);

        ButtonSelect.Play();
    }

    public void Controls()
    {
        //GUI.skin = MenuGUISkin;
        //float screenWidth = Screen.width;
        //float halfWidthScreen = screenWidth / 2;
        //
        //float screenHeight = Screen.height;
        //float percentage = 20;
        //float hudXPos = (screenWidth / 100) * percentage;
        //float hudYPos = (screenHeight / 100) * percentage;
        //float buttonXPos = ((hudXPos / 100) * percentage);
        //float buttonYPos = ((hudYPos / 100) * percentage);

        //Rect txtField = new Rect(hudXPos, hudYPos - (buttonYPos * 3), screenWidth - (2 * hudXPos), screenHeight);

        //string controlsTxt = "A   Move left\nD   Move right\nSpace   Jump\nLShift   Pick up rocks\nE   To pick up\nR   To make fire\nF   To use item\nV   To drop item\nP   Pause game";


        _Controls.SetActive(true);
        ButtonPlay.SetActive(false);
        ButtonQuit.SetActive(false);
        ButtonControls.SetActive(false);
        Title.SetActive(false);
        ButtonBack.SetActive(true);

        StartVideo(Movie1);
        StartVideo(Movie2);
        StartVideo(Movie3);
        StartVideo(Movie4);




        _Counter = 3;
        _BackZone = true;

        ButtonSelect.Play();

        // _Controls.SetActive(false);


        //GUI.TextArea(txtField, controlsTxt);
    }

    public void Quit()
    {
        ButtonSelect.Play();

        Application.Quit();
    }

    public void Back()
    {
        _Controls.SetActive(false);
         ButtonPlay.SetActive(true);
         ButtonQuit.SetActive(true);
         ButtonControls.SetActive(true);
         ButtonBack.SetActive(false);
        Title.SetActive(true);

        EndVideo(Movie1);
        EndVideo(Movie2);
        EndVideo(Movie3);
        EndVideo(Movie4);

        ButtonSelect.Play();

        _Counter = 0;
        _BackZone = false;
    }


    void StartVideo(GameObject Movie)
    {

        ButtonSelect.Play();

        Movie.SetActive(true);
        //var r = Movie.GetComponent<CanvasRenderer>();
        var image = Movie.GetComponent<Image>();
        MovieTexture movieTex = (MovieTexture)image.material.mainTexture;
        movieTex.loop = true;
        movieTex.Play();
    }


    void EndVideo(GameObject Movie)
    {

        ButtonSelect.Play();

        Movie.SetActive(false);
        var image = Movie.GetComponent<Image>();
        MovieTexture movieTex = (MovieTexture)image.material.mainTexture;
        movieTex.loop = false;
        movieTex.Stop();
    }

}




