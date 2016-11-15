#define WindowsBuild

using UnityEngine;
using System.Collections;

/*
// Handles input and activation of the pickup
*/
public class Char_PickupActivation : MonoBehaviour {
    #if WindowsBuild
    private Gen_ControllerInput _Controller;
    #endif
    bool _Activate = false;
    public AudioClip ShieldActivation;
	// Update is called once per frame
	public bool PickupUpdate (PickupType pickup, int player, bool Keyboard) {

        if(!Keyboard)
        playerInput(player);
        else
            _Activate = Input.GetKeyDown(KeyCode.Space);

        if (_Activate)
        {
            //Activate the pickup
            switch (pickup)
            {
                case PickupType.Grenade:
                    this.GetComponent<Char_Manager>()._Weapon = WeaponType.Grenades;
                    this.GetComponent<Char_Manager>()._BulletsInPickup = 3;
                    break;
                case PickupType.Swap:
                    if(this.GetComponent<Char_Manager>()._DefaultWeapon == WeaponType.Laser)
                        this.GetComponent<Char_Manager>()._Weapon = WeaponType.Shotgun;
                    else
                        this.GetComponent<Char_Manager>()._Weapon = WeaponType.Laser;
                    this.GetComponent<Char_Manager>()._BulletsInPickup = 5;
                    break;
                case PickupType.Mine:
                    this.GetComponent<Char_Manager>()._Weapon = WeaponType.Mines;
                    this.GetComponent<Char_Manager>()._BulletsInPickup = 2;
                    break;
                case PickupType.Shield:
                    transform.FindChild("ShieldSound").GetComponent<AudioSource>().Play();
                    this.GetComponent<Char_Manager>().Shield(3,true);
                    break;
                default:
                    this.GetComponent<Char_Manager>()._Weapon = WeaponType.Laser;
                    break;

                   
            }

            return true;

        }

        return false;

    }

    /*
    / Returns input of specefiek player
    */
    void playerInput(int playNum)
    {
    #if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(playNum);
        _Activate = _Controller.GetButtonDown("LB");
    #else
        _Activate = Input.GetButton("Activate" + playNum.ToString());
#endif

    }
}
