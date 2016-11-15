#define WindowsBuild

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


/*
// Handles end screen information
*/
public class Gen_End_UI_Script : MonoBehaviour {

    [System.Serializable]
    public class score
    {
        public int points;
        public string name;
        public Color color;
        public int deaths;
    }

    private List<score> _ScoreList = new List<score>();

    public Text[] PlayerName;
    public Text[] ScoreText;
    public Text[] DeathText;
    public Image[] ColorPlayerBack;
    public Button MenuButton;
    public Text Title;

    public Gen_Score_Check ScoreCheck;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    // Use this for initialization
    void Start ()
    {
        //get scores 
        for(int i = 0; i < 4; ++i)
        {
            score sc = new score();
            sc.points = ScoreCheck.GetPlayerScore(i+1);
            sc.deaths = ScoreCheck.GetPlayerDeath(i + 1);
            sc.name = GetComponent<Gen_ReadNames>().GetName(i);
            sc.color = SetColor(i+1);
            _ScoreList.Add(sc);
        }

        //check highest score and list them
        _ScoreList = _ScoreList.OrderBy(x => x.points).ToList();

        int count = 1;
        foreach(var score in _ScoreList)
        {
            switch ((int)count)
            {
                case 4:
                    PlayerName[0].text = score.name;
                    ScoreText[0].text = "" + score.points;
                    DeathText[0].text = "" + score.deaths;
                    ColorPlayerBack[0].color = score.color;
                    Title.color = score.color;
                    break;
                case 3:
                    PlayerName[1].text = score.name;
                    ScoreText[1].text = "" + score.points;
                    DeathText[1].text = "" + score.deaths;
                    ColorPlayerBack[1].color = score.color;
                    break;
                case 2:
                    PlayerName[2].text = score.name;
                    ScoreText[2].text = "" + score.points;
                    DeathText[2].text = "" + score.deaths;
                    ColorPlayerBack[2].color = score.color;
                    break;
                case 1:
                    PlayerName[3].text = score.name;
                    ScoreText[3].text = "" + score.points;
                    DeathText[3].text = "" + score.deaths;
                    ColorPlayerBack[3].color = score.color;
                    break;
            }
            ++count;
        }
    }

    void Update()
    {
#if WindowsBuild

        _Controller = Gen_ControllerManager.Instance.GetController(1);

        if (_Controller.IsConnected)
        {
            if (_Controller.GetButtonDown("A"))
            {
                ReDirect();
            }
        }
#else
        if (Input.GetButton("Ready1") || Input.GetButtonDown("Submit"))
        {
            ReDirect();
        }
#endif
    }

    Color SetColor(int playerNum)
    {
        switch(playerNum)
        {
            case 1:
                return new Color(77.0f / 255, 63.0f / 255, 244.0f / 255);
            case 2:
                return new Color(246.0f / 255, 197.0f / 255, 69.0f / 255);
            case 3:
                return new Color(211.0f / 255, 59.0f / 255, 71.0f / 255);
            case 4:
                return new Color(35.0f / 255, 200.0f / 255, 108.0f / 255);
            default:
                return Color.white;
        }
    }

    public void ReDirect()
    {
#if WindowsBuild
        Gen_ControllerManager.Instance.RemoveRumbleAll();
#endif
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
