#define WindowsBuild
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
// Handles player enableing readying and activation in the main level
*/
public class Gen_Intro_Script : MonoBehaviour {

    public GameObject[] Players;
    public GameObject[] PlayerText;
    
    public GameObject ContinueText;
    public GameObject[] PlayerReady;

    public GameObject LevelSelectMenu;

    public bool _IntroLevel = false;

    public static bool[] PlayerEnabled = new bool[4];
    public static WeaponType[] PlayerWeapon = new WeaponType[4];
    public static bool[] UsesKeyboard = new bool[4];
    public static int[] ContollerNum = new int[4];
    private bool[] _PlayerReady = new bool[4];

    private int _KeyboardUser = 0;
    private int _KeyboardUserId=8;
    private int _playerCount = 0;
    private int _ControllerCount = 1;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    // Use this for initialization
    void Start()
    {
        //Set everything false at start of level
        for (int i = 0; i < 4; ++i)
        {
            Players[i].SetActive(false);
            PlayerText[i].SetActive(false);

            Players[i].GetComponent<Char_Manager>().SetDefault(PlayerWeapon[i]);
            Players[i].GetComponent<Char_Manager>().UsesKeyboard = UsesKeyboard[i];
            Players[i].GetComponent<Char_Manager>().ControllerNum = ContollerNum[i];

            if (_IntroLevel)
            {
                PlayerEnabled[i] = false;
                _PlayerReady[i] = false;
                PlayerReady[i].SetActive(false);
            }
        }

        if (_IntroLevel)
        {
            ContinueText.SetActive(false);
            ContinueText.GetComponent<Text>().text = "Press A when ready";
        }
    }

	// Update is called once per frame
	void Update()
    {
       
     

        if (_IntroLevel)
        {
            MakeVisible();
            CheckGame();
            ReadyPlayers();
        }

        dropInPlayer();
        enablePlayers();
    }

    void dropInPlayer()
    {

#if WindowsBuild
        for(int i = 0; i < 4; ++i)
        {
            int playNum = i + 1;

            //WINDOWS
            _Controller = Gen_ControllerManager.Instance.GetController(playNum);

            if (_Controller.IsConnected)
            {
                if ((_Controller.GetButtonDown("A") || _Controller.GetButtonDown("RB")) && !PlayerEnabled[i])
                {
                    EnablePlayer(i, false, playNum);
                }
            }
        }

        if (/*_IntroLevel&&*/_KeyboardUser == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < 4; ++i)
                {
                    int playNum = i + 1;
                    if (!PlayerEnabled[i])
                    {
                        EnablePlayer(i, true, playNum);
                        _KeyboardUserId = i + 1;
                        Debug.Log(playNum);
                        break;
                    }

                }
            }
        }
#else

        for(int i=0;i<4;++i)
        {
            int playNum = i + 1-_KeyboardUser;
            if (playNum <= 4)
            {
                if (playNum != 0)
                {
                    if (((Input.GetButtonDown("Ready" + playNum.ToString()) || Input.GetAxis("Fire" + playNum.ToString()) > 0)) && !PlayerEnabled[i])
                    {
                        Debug.Log(playNum);
                        EnablePlayer(i, false, playNum);
                    }
                }
            }

        }

        if (/*_IntroLevel&&*/_KeyboardUser==0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < 4; ++i)
                {
                    int playNum = i + 1;
                    if (!PlayerEnabled[i])
                    {
                        EnablePlayer(i, true, playNum);
                        _KeyboardUserId = i + 1;
                        Debug.Log(playNum);
                        break;
                    }

                }
            }
        }

#endif
    }

    void enablePlayers()
    {
        for (int i = 0; i < 4; ++i)
        {
            if(PlayerEnabled[i])
            {
                Players[i].SetActive(true);
                PlayerText[i].SetActive(true);
            }
            else
            {
                Players[i].SetActive(false);
                PlayerText[i].SetActive(false);
            }
        }
    }

    void ReadyPlayers()
    {
        if (_playerCount > 1)
        {
            ContinueText.SetActive(true);
            for (int i = 0; i < 4; ++i)
            {
                int playNum = i + 1;

                //WINDOWS
#if WindowsBuild
                _Controller = Gen_ControllerManager.Instance.GetController(playNum);
                if (playNum == _KeyboardUserId)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && PlayerEnabled[i])
                    {
                        //PLAYER READY
                        _PlayerReady[i] = true;
                        PlayerReady[i].SetActive(true);
                    }
                }
                else if (_Controller.IsConnected)
                {
                    //PLAYER READY
                    if (_Controller.GetButtonDown("A") && PlayerEnabled[i])
                    {
                        _PlayerReady[i] = true;
                        PlayerReady[i].SetActive(true);
                    }
                    //PLAYER CANCEL
                    if (_Controller.GetButtonDown("B") && PlayerEnabled[i])
                    {
                        _PlayerReady[i] = false;
                        PlayerReady[i].SetActive(false);
                    }
                }
                //WEB
#else
                if (playNum == _KeyboardUserId)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && PlayerEnabled[i])
                    {
                        //PLAYER READY
                        _PlayerReady[i] = true;
                        PlayerReady[i].SetActive(true);
                    }
                }
                else
                {
                    if (playNum > _KeyboardUserId)
                    {
                        playNum -= 1;
                    }

                    if (Input.GetButtonDown("Ready" + playNum.ToString()) && PlayerEnabled[i])
                    {
                        //PLAYER READY
                        _PlayerReady[i] = true;
                        PlayerReady[i].SetActive(true);
                    }
                    if (Input.GetButtonDown("Cancel" + playNum.ToString()) && PlayerEnabled[i])
                    {
                        //PLAYER CANCEL READY
                        _PlayerReady[i] = false;
                        PlayerReady[i].SetActive(false);
                    }
                }
               
#endif
            }
    }
        else
        {
            ContinueText.SetActive(false);
        }
    }

    void CheckGame()
    {
        ContinueText.GetComponent<Text>().text = "Press A when ready";
        //check if enabled players are ready (not enabled players not ready)
        if (_playerCount > 1)
        {
            bool everyoneReady = false;
            for (int i = 0; i < 4; ++i)
            {
                if (PlayerEnabled[i] == _PlayerReady[i])
                {
                    everyoneReady = true;
                }
                else
                {
                    //Not enough players ready
                    everyoneReady = false;
                    break;
                }
            }
            if (everyoneReady)
            {
                ContinueText.GetComponent<Text>().text = "Player 1 press A to start";

#if WindowsBuild //WINDOWS
                _Controller = Gen_ControllerManager.Instance.GetController(1);
                if (_Controller.IsConnected)
                {
                    if (_Controller.GetButtonDown("A"))
                    {
                         LevelSelectMenu.SetActive(true);
                    }
                }
#else           //WEB
                if (Input.GetButtonDown("Ready1"))
                {
                    LevelSelectMenu.SetActive(true);
                }
#endif
            }
        }
    }

    void EnablePlayer(int PlayerNum,bool keyboard,int controller)
    {
        int index = PlayerNum;
        Players[index].SetActive(true);
        //Players[index].GetComponent<Char_Manager>().CreateCharacter(GetPlayerColor(PlayerNum), ControllerType.Controller, WeaponType.Laser);
        Players[index].GetComponent<Char_Manager>().SpawnCharacter();
        Players[index].GetComponent<Char_Manager>().UsesKeyboard = keyboard;
        Players[index].GetComponent<Char_Manager>().ControllerNum = controller;
        PlayerText[index].SetActive(true);
        PlayerEnabled[index] = true;
        UsesKeyboard[index] = keyboard;
        ContollerNum[index] = controller;
        if (_IntroLevel)
            _PlayerReady[index] = false;

        ++_playerCount;

        if (!keyboard)
            _ControllerCount++;
        else
            _KeyboardUser++;
    }

    PlayerColor GetPlayerColor(int PlayerNum)
    {
        switch (PlayerNum)
        {
            case 1:
                return PlayerColor.BLUE;
            case 2:
                return PlayerColor.YELLOW;
            case 3:
                return PlayerColor.RED;
            case 4:
                return PlayerColor.GREEN;
            default:
                return PlayerColor.BLUE;
        }
    }

    void MakeVisible()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (PlayerEnabled[i])
            {
                Players[i].GetComponent<Char_Vis>().MakeVis();
            }
        }
    }

    public bool IsPlayerReady(int playNum)
    {
        return _PlayerReady[playNum - 1];
    }

}
