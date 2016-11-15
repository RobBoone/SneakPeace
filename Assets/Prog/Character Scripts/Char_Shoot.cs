
#define WindowsBuild
using UnityEngine;
using System.Collections;

public class Char_Shoot : MonoBehaviour {
    /*
    // Every character has a weapon it can shoot.
    // This class holds all the functions nececary for the shooting behaviour.
    // Don't check user input in here !!!
    */

    
    float _FireDown = 0;
    float _FireUp = 0;

    float ShootTimer = -36;
    float ChargeTimer = -36;
    bool CanShoot = true;

    //Spawnable prefabs
    public GameObject LaserPrefab;
    public GameObject NadePrefab;
    public GameObject MinePrefab;
    public GameObject ShotGunEffect;
    
    //Cooldowns
    public float LaserCooldown;
    public float GrenadeCooldown;
    public float ShotGunCooldown;
    public float MineCooldown;

    //Weapon charge speed
    public float LaserCharge;
    public float GrenadeCharge;
    public float ShotgunCharge;
    public float MineCharge;

    //General charge progression
    public float ChargeSpeed;

    //Position from where the projectiles get shot
    public Transform Gunpoint;


    //Colors
    public Color AmmoColor;

    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color LaserColor;

    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color ShotGunColor;

    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color MineColor;

    //Audio of the weapons
    public AudioClip LaserSound;
    public AudioClip GrenadeThrowSound;
    public AudioClip ShotgunSound;
    public AudioClip SetMineSound;


#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    // Shoot should get called if a character can shoot and wants to do so.
    public void Shoot (WeaponType weapon, float strength = 0) {

        transform.FindChild("Model").GetComponent<Animator>().SetTrigger("Shoot");

        switch (weapon)
        {
            //Grenades bounce and have area of effect damage* when exploding
            case WeaponType.Grenades:
                ShootBomb(this.gameObject);        
                break;
            //Lasers are simple straight shots. (It can only hit one person)
            case WeaponType.Laser:
                ShootLaser(strength);
                break;  
            //Mines can be planted in the ground. They become invisible
            //Once someone steps on it they explode and deal area of effect damage*
            case WeaponType.Mines:
                ShootMine(this.gameObject);              
                break;
            //Shotguns fire a series of bullets in a cone (The bullets can hit more than one person!!)
            case WeaponType.Shotgun:
                ShootShotGun();                
                break;
        }

        GetComponent<Char_Vis>().Moved();
	}

    //Decides when a weapon gets shot
    public void Discharge( WeaponType weapon)
    {

        if(weapon == WeaponType.Laser && ChargeTimer > LaserCharge)
        {
            ChargeTimer = LaserCharge;
        }

        Shoot(weapon, ChargeTimer);
        GetComponent<Char_Manager>().ShotWeapon();
        ChargeTimer = -36;

        //Change particle settings
        var em = transform.FindChild("Charging").GetComponent<ParticleSystem>().emission;
        em.enabled = false;

        //transform.FindChild("Charging").GetComponent<ParticleSystem>().Pause();
    }

    /*
    / Checks the timers and if you can Charge, 
    / Returns true if this character can shoot
    */
    public void ChargeUpdate( WeaponType weapon)
    {

        //Debug.Log("charging");
        
        //Charge the weapon, discharge if released prematurely
        if (_FireUp > 0)
        {
            //Discharge weapon
            Discharge( weapon);
            return;
        }

        ChargeTimer += Time.deltaTime * ChargeSpeed;

        switch (weapon)
        {
            case WeaponType.Laser:
                if(ChargeTimer > LaserCharge)
                {
                    //Discharge(player, weapon);
                }
                break;
            case WeaponType.Shotgun:
                if (ChargeTimer > ShotgunCharge)
                {
                    Discharge( weapon);
                }
                break;
            case WeaponType.Mines:
                if (ChargeTimer > MineCharge)
                {
                    Discharge( weapon);
                }
                break;
            case WeaponType.Grenades:
                if (ChargeTimer > GrenadeCharge)
                {
                    Discharge( weapon);
                }
                break;
        }


    }

    /*
    / Checks the timers and if you can Charge, 
    / Returns true if this character can shoot
    */
    public void ShootUpdate(int player, WeaponType weapon,bool Keyboard)
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if (!Keyboard)
            playerInput(player);
        else
        {
            _FireDown = Input.GetKeyDown(KeyCode.Mouse0)? 1 : 0;
            _FireUp = Input.GetKeyUp(KeyCode.Mouse0) ? 1 : 0;
        }


        if (ChargeTimer != -36)
        {
            ChargeUpdate( weapon);
            return;
        }

        //Shoot Cooldown Timer
        if (ShootTimer >= 0 && ShootTimer != -36)
        {
            ShootTimer -= Time.deltaTime;
            if (ShootTimer <= 0)
            {
                CanShoot = true;
                ShootTimer = -36;
            }
        }

        if (_FireDown > 0 && CanShoot)
        {
            CanShoot = false;
            ChargeTimer = 0;

            //Change particle settings
            var em = transform.FindChild("Charging").GetComponent<ParticleSystem>().emission;
            em.enabled = true;

            //transform.FindChild("Charging").GetComponent<ParticleSystem>().Play();

            return;
        }

        return;
    }

    void playerInput(int playNum)
    {
#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(playNum);
            if (_Controller.IsConnected)
            {
                //_Fire = _Controller.GetButton("RB") ? 1 : 0;
                _FireDown = _Controller.GetButtonDown("RB") ? 1 : 0;
                _FireUp = _Controller.GetButtonUp("RB") ? 1 : 0;
            }
            else if (playNum == 3)
            {
                //_Fire = Input.GetButton("Fire3") ? 1 : 0;
                _FireDown = Input.GetButtonDown("Fire3") ? 1 : 0;
            }
            else if (playNum == 4)
            {
                //_Fire = Input.GetButton("Fire4") ? 1 : 0;
                _FireDown = Input.GetButtonDown("Fire4") ? 1 : 0;
            }
#else
            //_Fire = Input.GetButton("Fire" + playNum.ToString()) ? 1 : 0;
            _FireDown = Input.GetButtonDown("Fire" + playNum.ToString()) ? 1 : 0;
            _FireUp = Input.GetButtonUp("Fire" + playNum.ToString()) ? 1 : 0;
#endif
    }


    /*
    // Individual behaviour of the weapons
    */

    void ShootShotGun()
    {
        GetComponent<AudioSource>().clip = ShotgunSound;
        GetComponent<AudioSource>().Play();

        Vector3 StraigtDirection = transform.forward;
        RaycastHit hit;

        for (int i = -4; i <= 4; i++)
        {
            Vector3 AngleDirection = Quaternion.Euler(12*i, 12* i, 12*i) * StraigtDirection;
            AngleDirection.y = 0;
             //Debug.DrawRay(direction.origin, direction.direction);

             Ray Direction = new Ray(transform.position, AngleDirection);

            //Visual Behaviour (Drawing the ray)
            //DrawRay(transform.position, AngleDirection * 5.0f + transform.position);

            //Instantiate the shotgun effect
            var t = Instantiate(ShotGunEffect, Gunpoint.position, transform.rotation) as GameObject;
            t.transform.FindChild("Muzzle").GetComponent<Renderer>().material.SetColor("_EmissionColor", ShotGunColor);

            if (Physics.Raycast(Direction, out hit, 5.0f))
            {
                //Hit detection
                if (hit.collider.tag == "Player" 
                    && 
                    hit.collider.gameObject != this.gameObject 
                    && 
                    !hit.collider.gameObject.GetComponent<Char_Manager>()._Shielded)
                {
                    //Someone got hit here
                    GetComponent<Char_Manager>().HitSomeone(hit.collider.gameObject);
                    hit.collider.gameObject.GetComponent<Char_Manager>().GotHit(transform, gameObject);
                }
            }
        }

        ShootTimer = ShotGunCooldown;
    }

    void DrawRay(Vector3 begin, Vector3 end)
    {
        GameObject t = Instantiate(LaserPrefab) as GameObject;
        t.GetComponent<LineRenderer>().SetPosition(0, begin);
        t.GetComponent<LineRenderer>().SetPosition(1, end);

        //t.GetComponent<LineRenderer>().SetColors(LaserColor, LaserColor);
        t.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", LaserColor);
    }

    void ShootLaser(float strength)
    {
        GetComponent<AudioSource>().clip = LaserSound;
        GetComponent<AudioSource>().Play();

        //Shoot ray in a straigt line in the direction of this.
        Ray Direction = new Ray(Gunpoint.position, transform.forward);
        RaycastHit hit;

        strength *= 65;

        //Hit Test
        if (Physics.Raycast(Direction, out hit, strength))
        {
            //Visual Behaviour
            DrawRay(Gunpoint.position, hit.point);

            if (hit.collider.tag == "Player" 
                && 
                hit.collider.gameObject != this.gameObject 
                &&
                !hit.collider.gameObject.GetComponent<Char_Manager>()._Shielded)
            {
                //Someone got hit here
                GetComponent<Char_Manager>().HitSomeone(hit.collider.gameObject);
                hit.collider.gameObject.GetComponent<Char_Manager>().GotHit(transform, gameObject);
            }
        }
        else
        {
            DrawRay(Gunpoint.position, Gunpoint.position + transform.forward * strength);
        }

        ShootTimer = LaserCooldown;
    }
    
    public void ShootBomb(GameObject playerWhoShoots)
    {
        

        // v = Vector3.(v, dir);
        GameObject nade = Instantiate(NadePrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation) as GameObject;
        nade.transform.FindChild("M_Granade_1").GetComponent<Renderer>().material.color = AmmoColor;
        nade.GetComponent<Env_GrenadeScript>().playerColor = AmmoColor;
        //nade.transform.LookAt(_indicator.transform.position); // _Indicator does not exist
        Vector3 direction = transform.forward;
        direction.y =30*Mathf.Deg2Rad;
        nade.GetComponent<Rigidbody>().AddForce(direction * 3000);// = v * Mathf.Sqrt(Range * Physics.gravity.magnitude / Mathf.Sin(2 * Angle * Mathf.Deg2Rad));
        nade.transform.Find("TempGrenadeIndicator").GetComponent<Env_DetonationScript>().playerToScore = playerWhoShoots;

        ShootTimer = GrenadeCooldown;
    }

    public void ShootMine(GameObject playerWhoShoots)
    {
        //GetComponent<AudioSource>().clip = SetMineSound;
        //GetComponent<AudioSource>().Play();


        var t = Instantiate(MinePrefab, transform.position + new Vector3(0,-0.9f,0), Quaternion.identity) as GameObject;
        t.GetComponent<Env_Mine>().playerToScore = playerWhoShoots;
        t.GetComponent<Env_Mine>().playerColor = AmmoColor;
        t.transform.FindChild("Base").GetComponent<Renderer>().material.color = AmmoColor;
        t.transform.FindChild("Indicator").GetComponent<Renderer>().material.SetColor("_EmissionColor", MineColor);

        ShootTimer = MineCooldown;
    }
}
