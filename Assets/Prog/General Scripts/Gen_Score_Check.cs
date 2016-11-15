using UnityEngine;
using System.Collections;

public class Gen_Score_Check : MonoBehaviour
{

    public GameObject[] Players;

    public float MaxScore;      // Score needed to win

    static int[] _PlayerKills = new int[4];
    static int[] _PlayerDeaths = new int[4];

    public bool EndScene;
   

    // Use this for initialization
    void Start()
    {
        if(!EndScene)
        {
            UpdateKillCounters();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!EndScene)
        {
            UpdateKillCounters();
            UpdateDeathCounters();
            ScoreCheck();
        }
    }

    void UpdateKillCounters()
    {
        _PlayerKills[0] = Players[0].GetComponent<Char_Manager>().GetScore();
        _PlayerKills[1] = Players[1].GetComponent<Char_Manager>().GetScore();
        _PlayerKills[2] = Players[2].GetComponent<Char_Manager>().GetScore();
        _PlayerKills[3] = Players[3].GetComponent<Char_Manager>().GetScore();
    }

    void UpdateDeathCounters()
    {
        _PlayerDeaths[0] = Players[0].GetComponent<Char_Manager>().GetDeaths();
        _PlayerDeaths[1] = Players[1].GetComponent<Char_Manager>().GetDeaths();
        _PlayerDeaths[2] = Players[2].GetComponent<Char_Manager>().GetDeaths();
        _PlayerDeaths[3] = Players[3].GetComponent<Char_Manager>().GetDeaths();
    }

    void ScoreCheck()
    {
        if (_PlayerKills[0] >= MaxScore || _PlayerKills[1] >= MaxScore || _PlayerKills[2] >= MaxScore || _PlayerKills[3] >= MaxScore)
        {
            //Gen_ControllerManager.Instance.RemoveRumbleAll();
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
    }

    public int GetPlayerScore(int playerNumber)
    {
        switch((int)playerNumber)
        {
            case 1:
                return _PlayerKills[0];
            case 2:
                return _PlayerKills[1];
            case 3:
                return _PlayerKills[2];
            case 4:
                return _PlayerKills[3];
        }
        return -1;
    }

    public int GetPlayerDeath(int playerNumber)
    {
        switch ((int)playerNumber)
        {
            case 1:
                return _PlayerDeaths[0];
            case 2:
                return _PlayerDeaths[1];
            case 3:
                return _PlayerDeaths[2];
            case 4:
                return _PlayerDeaths[3];
        }
        return -1;
    }
}
