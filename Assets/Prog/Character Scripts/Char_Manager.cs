#define WindowsBuild
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public enum PlayerColor
{
    BLUE,
    YELLOW,
    RED,
    GREEN
}

public enum ControllerType
{
    KB,
    Controller
}

public enum WeaponType
{
    Laser,
    Shotgun,
    Grenades,
    Mines
}

public enum LifeState
{
    Alive,
    Dying,
    Death,
    Respawning
}

public enum PickupType
{
    None,
    Grenade,
    Swap,
    Mine,
    Shield,
}

public class Char_Manager : MonoBehaviour {
    /*
    //This class manages the character
    //
    //Initialisation + Calls to all other character scripts should happen from here.
    //Don't call other character scripts from anywhere else as that will create dependency loops
    //(Not wrong , just anoying to deal with)
    */
    
    //Character Properties
    public PlayerColor _PlayerColor;
    public ControllerType _PlayerController;
    public WeaponType _Weapon;
    public WeaponType _DefaultWeapon;
    public LifeState _LifeState;

    //Spawning and dying
    public Transform[] _RespawnPoints;
    public Transform _DeathPoint;
    public float SpawnProtection;

    //Ui and player feedback links
    public Text _ScoreBoard;
    public Text _PickupText;
    int _Score;
    int _Deaths;

    //Childs Scripts
    Char_Vis VisScript;
    Char_Shoot ShootScript;
    Char_Move MoveScript;
    Char_Death DeathScript;
    Env_Pause PauseScript;

    public bool _BecomeInvis = true;

    //Pickup variables
    public PickupType _AvailablePickup;
    public bool _HasPickup;
    public int _BulletsInPickup = 0;

    //Shield Variables
    public bool _Shielded;
    float _shieldTimer = -36;
    public float ShieldTime = 0.3f;
    public GameObject ShieldMesh;
    GameObject ShieldInst;
    private float _RandomShieldRot;

    //Despawning
    public GameObject DespawnObject;

    //The color of the spawn particles (CUA allows for hdr input)
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 2.5f)]
    public Color SpawnColor;
    public Color KillerColor;

    //weapon hold
    public GameObject _Sniper;
    public GameObject _Shotgun;

    //Keyboard
    public  bool UsesKeyboard = false;
    public int ControllerNum;

    //Controllers trough Xinput work only in windows builds (.exe)
#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif


    public void Start()
    {
        VisScript = GetComponent<Char_Vis>();
        ShootScript = GetComponent<Char_Shoot>();
        MoveScript = GetComponent<Char_Move>();
        DeathScript = GetComponent<Char_Death>();
        PauseScript = GameObject.Find("LevelEssentials").GetComponent<Env_Pause>();
        VisScript.MakeInvis();
        _LifeState = LifeState.Respawning;
        SpawnCharacter();
        _Shielded = false;
    }

    /*
    //Sets the default weapon type for this character.
    */
    public void SetDefault(WeaponType weapon)
    {
        _Weapon = weapon;
        _DefaultWeapon = weapon;
    }


    /*public void CreateCharacter(PlayerColor color, ControllerType type, WeaponType weapon)
    {
        _PlayerColor = color;
        _PlayerController = type;
        _Weapon = weapon;
        _Score = 0;
        _LifeState = LifeState.Respawning;
    }*/

    public void SpawnCharacter()
    {

        //Can only spawn characters which are allowed to spawn.
        if (_LifeState != LifeState.Respawning)
            return;

        //
        //Initialisation of the character
        //

        //Position the character at one of the spawnpoints
        for (int i = 0; i < _RespawnPoints.Length; ++i)
        {
            int position = UnityEngine.Random.Range(0, _RespawnPoints.Length);
            if (_RespawnPoints[position].GetComponent<Gen_SpawnPoint>().IsValid)
            {
                transform.position = _RespawnPoints[position].position;
                break;
            }
        }

        //Particles
        //Change color
        transform.FindChild("SpawnCircle").GetComponent<Renderer>().material.SetColor("_EmisionColor", SpawnColor);
        transform.FindChild("SpawnCircle").GetComponent<ParticleSystem>().Play();

        //Make the character visible
        VisScript.MakeVis();

        //If we are not in the intro screen, make the character disapear after a few seconds
        if (_BecomeInvis)
            VisScript.setVisTimer(1);

        //Set State to alive so character can get hit and can move
        _LifeState = LifeState.Alive;

        //Protect against spawndeath.
        Shield(SpawnProtection);

    }

    /*
    // Handles getting hit by other player
    */
    public void GotHit(Transform killer, GameObject pToScore)
    {
        //You can only get hit and die if you are alive
        if (_LifeState != LifeState.Alive)
            return;


        DeathScript.Dying(LifeState.Dying);

        //SetKillerColor
        KillerColor = pToScore.GetComponent<Char_Manager>().SpawnColor;

        //Bood flows to where you shoot
        //Theres probably a more elegant solution for this
        //This only rotates the particle system. The system only gets activated when dying!
        Vector3 impact = transform.position - killer.position;
        impact.Normalize();

        transform.FindChild("Blood").LookAt(transform.position + impact);

        ++_Deaths;

        transform.FindChild("Model").GetComponent<Animator>().SetTrigger("Death");
    }


    /*
    // Handles hitting someone
    */
    public void HitSomeone(GameObject hit)
    {
        //Can only hit someone who is alive and is not myself
        if (hit.GetComponent<Char_Manager>()._LifeState != LifeState.Alive || hit == this.gameObject)
            return;

        //Get more points!
        ++_Score;
        _ScoreBoard.text = _Score.ToString();

    }

    /*
    // Handles dying 
    ///Controller rumble only works on windows builds (xinput)
    */
    public void Dying()
    {
        //VisScript.MakeVisDeath();
        _LifeState = LifeState.Dying;

#if WindowsBuild
        switch (_PlayerColor)
        {
            case PlayerColor.BLUE:
                _Controller = Gen_ControllerManager.Instance.GetController(1);
                break;
            case PlayerColor.YELLOW:
                _Controller = Gen_ControllerManager.Instance.GetController(2);
                break;
            case PlayerColor.RED:
                _Controller = Gen_ControllerManager.Instance.GetController(3);
                break;
            case PlayerColor.GREEN:
                _Controller = Gen_ControllerManager.Instance.GetController(4);
                break;
        }
        if (_Controller.IsConnected)
        {
            //Debug.Log("rumble");
            _Controller.AddRumble(1.5f, new Vector2(0.75f, 0.75f), 0.5f);
        }
        
#endif
        
        //Displays the Particles
        transform.FindChild("Blood").GetComponent<ParticleSystem>().Play();

        


    }

    /*
    // Handles Being death awaiting res
    */
    public void Death()
    {
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        var t = Instantiate(DespawnObject, pos , Quaternion.identity) as GameObject;
        changeDespawnColor(t);

        _shieldTimer = -36;
        _Shielded = false;
        Destroy(ShieldInst);

        transform.position = _DeathPoint.transform.position;
        _LifeState = LifeState.Death;
    }

    //Changes the color of the despawn particles
    public void changeDespawnColor(GameObject t)
    {
        t.transform.FindChild("Despawning").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (1)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (2)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (3)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (4)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (5)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (6)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (7)").GetComponent<ParticleSystem>().startColor = KillerColor;
        t.transform.FindChild("Despawning").FindChild("Despawning (8)").GetComponent<ParticleSystem>().startColor = KillerColor;
    }

    /*
    // Shields someone for given amount of time or ShieldTime;
    */
    public void Shield(float time = -36, bool showMesh = false)
    {
        if (_LifeState != LifeState.Alive)
            return;

        _Shielded = true;
        if (time != -36)
            _shieldTimer = time;
        else
            _shieldTimer = ShieldTime;

        

        ShieldInst = Instantiate(ShieldMesh, transform.position, Quaternion.identity) as GameObject;
        ShieldInst.transform.SetParent(transform);
        ShieldInst.GetComponent<Renderer>().material.SetColor("_EmissionColor", GetComponent<Char_Shoot>().LaserColor);
        _RandomShieldRot = UnityEngine.Random.Range(-270.0f, 270.0f);

        if (!showMesh)
        {
            ShieldInst.GetComponent<Renderer>().enabled = false;
        }

    }

    /*
    // Getters and setters
    */
    public int GetScore()
    {
        return _Score;
    }

    public int GetDeaths()
    {
        return _Deaths;
    }

    /*
    // Handles picking something up
    */
    public void PickUp()
    {
        transform.FindChild("PickupSound").GetComponent<AudioSource>().Play();

        var amount = Enum.GetNames(typeof(PickupType)).Length;
    
        int t = Mathf.FloorToInt(UnityEngine.Random.Range(1, amount));
        
        _PickupText.text = Enum.GetName(typeof(PickupType), t);

        // Weapon pickup swaps between default weapon and other option
        if (_PickupText.text.Equals("Swap"))
        {
            if (_DefaultWeapon == WeaponType.Laser)
                _PickupText.text = "Shotgun";
            else
                _PickupText.text = "Laser";
        }
    
        _AvailablePickup = (PickupType)t;
        _HasPickup = true;

    }
	
    public void ShotWeapon()
    {
        if (_Weapon != _DefaultWeapon)
        {
            --_BulletsInPickup;
            if (_BulletsInPickup == 0)
            {
                //disable text and available pickup
                _Weapon = _DefaultWeapon;
                _AvailablePickup = 0;
                _PickupText.text = "";
                _BulletsInPickup = 0;
                _HasPickup = false;
            }
        }

        VisScript.MakeVis();
        if (_BecomeInvis)
            VisScript.setVisTimer(1);
    }

    void setVisibleWeapon(WeaponType weapon)
    {
        if(weapon == WeaponType.Laser)
        {
            _Sniper.SetActive(true);
            _Shotgun.SetActive(false);
        }
        else if (weapon == WeaponType.Shotgun)
        {
            _Sniper.SetActive(false);
            _Shotgun.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {

        if (_LifeState == LifeState.Respawning)
        {
            SpawnCharacter();
        }
        
        if(_LifeState != LifeState.Dying)
        {
            MoveScript.MoveUpdate(ControllerNum,UsesKeyboard);
            VisScript.VisibleUpdate();

            ShootScript.ShootUpdate(ControllerNum, _Weapon, UsesKeyboard);

            if (_HasPickup)
            {
                
                if (GetComponent<Char_PickupActivation>().PickupUpdate(_AvailablePickup, ControllerNum, UsesKeyboard))
                {
                    _HasPickup = false;
                    _PickupText.text = "";
                    _AvailablePickup = 0;
                }
            }

        }

        PauseScript.Pause(ControllerNum, UsesKeyboard);

        //Update the shield
        if (_shieldTimer != -36)
        {
            _shieldTimer -= Time.deltaTime;
            ShieldInst.transform.Rotate(new Vector3(0.0f, _RandomShieldRot, 0.0f)*Time.deltaTime) ;
            if (_shieldTimer <= 0)
            {
                _shieldTimer = -36;
                _Shielded = false;
                //transform.FindChild("SpawnCircle").GetComponent<ParticleSystem>().Stop();
                ShieldInst.GetComponent<Renderer>().enabled = false;
                Destroy(ShieldInst);
            }
        }

        //set visible weapon
        if(_Weapon == WeaponType.Laser || _Weapon == WeaponType.Shotgun)
        {
            setVisibleWeapon(_Weapon);
        }

    }
    
}
