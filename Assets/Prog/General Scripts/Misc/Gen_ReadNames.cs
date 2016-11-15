#define WindowsBuild

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class Gen_ReadNames : MonoBehaviour {

    public TextAsset NamesTextFile;

    public GameObject[] PlayerNamesText;

    public bool IntroScene;
    public bool EndScene;

    private string _FileNames;
    public static List<string> Names;
    public static int[] NameIndexes = new int[4] { -1,-1,-1,-1 };
    private float _Counter = 0;

#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif
    // Use this for initialization
    void Start ()
    {
        if(IntroScene && Names == null)
        {
            _FileNames = NamesTextFile.text;
            Names = new List<string>();
            Names.AddRange(_FileNames.Split("\n"[0]));
            Names.Sort();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!EndScene)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (IntroScene && PlayerNamesText[i].activeSelf && !gameObject.GetComponent<Gen_Intro_Script>().IsPlayerReady(i + 1))
                {
                    chooseName(i);
                }

                //set name on screen
                if (PlayerNamesText[i].activeSelf)
                {
                    PlayerNamesText[i].GetComponent<Text>().text = GetName(i);
                }
            }
        }
    }

    void chooseName(int index)
    {
        int playNum = index + 1;

        _Counter -= Time.deltaTime;

#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(playNum);
        if (_Controller.IsConnected)
        {
            if(_Controller.GetButtonDown("DPad_Left"))
            {
                if(NameIndexes[index] <= 0)
                {
                    NameIndexes[index] = Names.Count - 1;
                }
                else
                {
                    --NameIndexes[index]; 
                }
            }
            if (_Controller.GetButtonDown("DPad_Right"))
            {
                if (NameIndexes[index] >= Names.Count - 1)
                {
                    NameIndexes[index] = 0;
                }
                else
                {
                    ++NameIndexes[index];
                }
            }
            //Random name
            if (_Controller.GetButtonDown("Y"))
            {
                NameIndexes[index] = Random.Range(0, Names.Count);
            }
       
        }
#else
        float dpadX = getDpadX(playNum);
        if (dpadX < 0.0f&&_Counter<0.0f)
        {
            if (NameIndexes[index] <= 0)
            {
                NameIndexes[index] = Names.Count - 1;
            }
            else
            {
                --NameIndexes[index];
            }
            _Counter = 0.7f;
        }
        if (dpadX > 0.0f&&_Counter < 0.0f)
        {
            if (NameIndexes[index] >= Names.Count - 1)
            {
                NameIndexes[index] = 0;
            }
            else
            {
                ++NameIndexes[index];
            }
            _Counter = 0.7f;
        }
        //Random name
        if (Input.GetButtonDown("YButton" + playNum.ToString()))
        {
            NameIndexes[index] = Random.Range(0, Names.Count);
        }
#endif
    }

    public string GetName(int index)
    {
        if (NameIndexes[index] == -1)
        {
            return ("PLAYER " + (index + 1).ToString());
        }
        else
        {
            return Names[NameIndexes[index]].ToUpper();
        }

    }

    float getDpadX(int playNum)
    {
        float x = Input.GetAxis("DpadX" + playNum.ToString()); ; 
        return x;
    }
}
