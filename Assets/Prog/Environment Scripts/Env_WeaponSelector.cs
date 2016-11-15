using UnityEngine;
using System.Collections;

public enum WeaponChoice
{
    Sniper,
    Shotgun
}

/*
// Handles weapon selection and visibility
*/
public class Env_WeaponSelector : MonoBehaviour
{

    public WeaponChoice WeaponSelect = WeaponChoice.Sniper;
    //public PlayerColor PlayerCol;

    private GameObject _Gun;
    private Color[] _Colors = new Color[5];

    // Use this for initialization
    void Start()
    {
        enableWeapon(WeaponSelect);

        _Colors[0] = new Color(77.0f / 255, 63.0f / 255, 244.0f / 255, 3.0f) * 10.0f;
        _Colors[1] = new Color(246.0f / 255, 197.0f / 255, 69.0f / 255, 3.0f) * 10.0f;
        _Colors[2] = new Color(211.0f / 255, 59.0f / 255, 71.0f / 255, 3.0f) * 10.0f;
        _Colors[3] = new Color(35.0f / 255, 200.0f / 255, 108.0f / 255, 3.0f) * 10.0f;
        _Colors[4] = new Color(77.0f / 255, 63.0f / 255, 244.0f / 255, 3.0f) * 10.0f;
        //transform.Find("Color").GetComponent<Renderer>().material.SetColor("_EmissionColor", GetColor(PlayerCol));
    }

    // Update is called once per frame
    void Update()
    {
        _Gun.transform.Rotate(new Vector3(0.0f, Random.Range(-15.0f,-180.0f)) * Time.deltaTime);

        float t = Mathf.Repeat(Time.time, 2.0f) / 2.0f;
        transform.Find("Color").GetComponent<Renderer>().material.SetColor("_EmissionColor", AdvancedColorLerp(t,_Colors));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<Char_Manager>()._Weapon = getWeaponType(WeaponSelect);
            Gen_Intro_Script.PlayerWeapon[GetIndex(other.gameObject.GetComponent<Char_Manager>()._PlayerColor)] = getWeaponType(WeaponSelect);
        }
    }

    void enableWeapon(WeaponChoice weapon)
    {
        if (weapon == WeaponChoice.Sniper)
        {
            _Gun = transform.Find("Sniper").gameObject;
            transform.Find("Shotgun").gameObject.SetActive(false);
        }
        else if (weapon == WeaponChoice.Shotgun)
        {
            _Gun = transform.Find("Shotgun").gameObject;
            transform.Find("Sniper").gameObject.SetActive(false);
        }
        _Gun.SetActive(true);
        //_Gun.transform.Rotate(0.0f, Random.Range(-180.0f, 180.0f), 0.0f);
    }

    WeaponType getWeaponType(WeaponChoice weapon)
    {
        if (weapon == WeaponChoice.Sniper)
        {
            return WeaponType.Laser;
        }
        else if (weapon == WeaponChoice.Shotgun)
        {
            return WeaponType.Shotgun;
        }
        return WeaponType.Laser;
    }

    int GetIndex(PlayerColor player)
    {
        switch (player)
        {
            case PlayerColor.BLUE:
                return 0;
            case PlayerColor.YELLOW:
                return 1;
            case PlayerColor.RED:
                return 2;
            case PlayerColor.GREEN:
                return 3;
            default:
                return 0;
        }
    }

    public Color AdvancedColorLerp(float t, params Color[] colors)
    {
        int c = colors.Length - 1; // number of colors it needs to transition to
        t = Mathf.Clamp01(t) * c; // ex[amd t frp, 0 to c
        int index = (int)Mathf.Clamp(Mathf.Floor(t), 0, c - 1);

        t -= index; // substract index to get back a value from 0-1
        return Color.Lerp(colors[index],colors[index+1],t);
    }
}
