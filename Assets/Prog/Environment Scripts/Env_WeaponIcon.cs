using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
// Changes Icons when character owns diferent weapon
*/
public class Env_WeaponIcon : MonoBehaviour {

    public Char_Manager Character;
    public float ShieldPingSpeed = 2.0f;

    private GameObject _Laser;
    private GameObject _Shotgun;
    private GameObject _Mine;
    private GameObject _Grenade;
    private Image _Shield;

    private WeaponType _PreviousWeapon;
    private Color _Color;

    // Use this for initialization
    void Start ()
    {
        Character = Character.GetComponent<Char_Manager>();

        _Laser = transform.Find("Sniper").gameObject;
        _Laser.SetActive(false);

        _Shotgun = transform.Find("Shotgun").gameObject;
        _Shotgun.SetActive(false);

        _Mine = transform.Find("Mine").gameObject;
        _Mine.SetActive(false);

        _Grenade = transform.Find("Grenade").gameObject;
        _Grenade.SetActive(false);

        _Shield = transform.Find("Shield").gameObject.GetComponent<Image>();
        _Color = _Shield.color;
        _Color.a = 0.0f;
        _Shield.color = _Color;

        _PreviousWeapon = WeaponType.Mines;
        EnableWeapon(Character._Weapon);
    }
	
	// Update is called once per frame
	void Update ()
    {
        WeaponType weapon = Character._Weapon;
        EnableWeapon(weapon);
        
        if(Character._Shielded)
        {
            _Color.a = Mathf.PingPong(Time.time * ShieldPingSpeed, 1);
            _Shield.color = _Color;
        }
        else if(_Color.a != 0.0f)
        {
            _Color.a = 0.0f;
            _Shield.color = _Color;
        }
	}

    void EnableWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Laser:
                if (!_Laser.activeSelf)
                {
                    _Laser.SetActive(true);
                    DisableWeapon(_PreviousWeapon);
                    _PreviousWeapon = WeaponType.Laser;
                }
                break;
            case WeaponType.Shotgun:
                if (!_Shotgun.activeSelf)
                {
                    _Shotgun.SetActive(true);
                    DisableWeapon(_PreviousWeapon);
                    _PreviousWeapon = WeaponType.Shotgun;
                }
                break;
            case WeaponType.Grenades:
                if (!_Grenade.activeSelf)
                {
                    _Grenade.SetActive(true);
                    DisableWeapon(_PreviousWeapon);
                    _PreviousWeapon = WeaponType.Grenades;
                }
                break;
            case WeaponType.Mines:
                if (!_Mine.activeSelf)
                {
                    _Mine.SetActive(true);
                    DisableWeapon(_PreviousWeapon);
                    _PreviousWeapon = WeaponType.Mines;
                }
                break;
            default:
                if (!_Laser.activeSelf)
                {
                    _Laser.SetActive(true);
                    DisableWeapon(_PreviousWeapon);
                    _PreviousWeapon = WeaponType.Laser;
                }
                break;
        }
    }

    void DisableWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Laser:
                _Laser.SetActive(false);
                break;
            case WeaponType.Shotgun:
                _Shotgun.SetActive(false);
                break;
            case WeaponType.Grenades:
                _Grenade.SetActive(false);
                break;
            case WeaponType.Mines:
                _Mine.SetActive(false);
                break;
            default:
                break;
        }
    }
}
