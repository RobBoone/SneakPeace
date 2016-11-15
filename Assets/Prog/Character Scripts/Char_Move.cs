#define WindowsBuild
using UnityEngine;
using System.Collections;

/*
// Handles movement and input of the characters
*/
public class Char_Move : MonoBehaviour {

    //speed modifiers
    public float ForwardSpeed = 15;
    public float StraveSpeed = 15;
    public float RotationMultiplier = 4;

    public Transform LevelAxis;
    private Vector3 Lookat;

    CharacterController CharacterController;

    float _LeftStickX = 0;
    float _LeftStickY = 0;
    float _RightStickX = 0;
    float _RightStickY = 0;
#if WindowsBuild
    private Gen_ControllerInput _Controller;
#endif

    // Use this for initialization
    void Start () {

        CharacterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    public void MoveUpdate(int player,bool Keyboard)
    {

        if (Keyboard)
        {

            GetComponent<Char_Movement_Mouse>().OverUpdate();

        }

        /*
        / Decide on input scheme depending on player
        */
        if(!Keyboard)
        playerInput(player);

        /*
        // PLAYER MOVEMENT
        */
        Vector3 movement = LevelAxis.right * _LeftStickX * Time.deltaTime * StraveSpeed;
        movement += LevelAxis.forward * _LeftStickY * Time.deltaTime * ForwardSpeed;

        if (((_RightStickX >= 0.1 || _RightStickX <= -0.1) || (_RightStickY >= 0.1 || _RightStickY <= -0.1)) && Time.timeScale != 0)
        {
            Vector3 direction = new Vector3(_RightStickX, 0, _RightStickY);
            if (direction.magnitude > 0.4f && direction != Vector3.zero)
            {
                Lookat = direction;
            }
        }

        if ((_LeftStickX >= 0.1 || _LeftStickX <= -0.1) || (_LeftStickY >= 0.1 || _LeftStickY <= -0.1))
        {
            GetComponent<Char_Vis>().Moved();
            transform.FindChild("Model").GetComponent<Animator>().SetBool("Running", true);

        }
        else
        {
            transform.FindChild("Model").GetComponent<Animator>().SetBool("Running", false);
        }

        transform.LookAt(Lookat + transform.position);

        if (!CharacterController.isGrounded)
        {
            movement += -transform.up;
        }

        if (movement.x == 0 && movement.z == 0)
        {
            
            if (transform.FindChild("WalkingSound").GetComponent<AudioSource>().isPlaying)
            {
                transform.FindChild("WalkingSound").GetComponent<AudioSource>().Stop();
                
            }
        }
        else
        {
            
            if (!transform.FindChild("WalkingSound").GetComponent<AudioSource>().isPlaying)
            {
                transform.FindChild("WalkingSound").GetComponent<AudioSource>().Play();
                
            }
        }
       // Debug.Log(movement);
        CharacterController.Move(movement);

    }



    /*
    / Returns input of specific player
    */
    void playerInput(int playNum)
    {
#if WindowsBuild
        _Controller = Gen_ControllerManager.Instance.GetController(playNum);
            if (_Controller.IsConnected)
            {
                _LeftStickX = _Controller.GetStick_L().X;
                _LeftStickY = _Controller.GetStick_L().Y;
                _RightStickX = _Controller.GetStick_R().X;
                _RightStickY = _Controller.GetStick_R().Y;
            }
#else
       

            _LeftStickX = Input.GetAxis("LeftStickX" + playNum.ToString());
            _LeftStickY = Input.GetAxis("LeftStickY" + playNum.ToString());
            _RightStickX = Input.GetAxis("RightStickX" + playNum.ToString());
            _RightStickY = Input.GetAxis("RightStickY" + playNum.ToString());
#endif
    }
}
