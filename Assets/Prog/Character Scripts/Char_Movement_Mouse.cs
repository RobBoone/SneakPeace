using UnityEngine;
using System.Collections;

/*
// Handles movement for the char controlled by the keyboard
*/
public class Char_Movement_Mouse : MonoBehaviour {
 
    CharacterController _characterController;
    Transform _levelAxis;

    //float _forwardSpeed;
    float _straveSpeed;

    // Use this for initialization
    void Start () {

        _characterController = GetComponent<CharacterController>();
        _levelAxis = GetComponent<Char_Move>().LevelAxis;
        //_forwardSpeed = GetComponent<Char_Move>().ForwardSpeed;
        _straveSpeed = GetComponent<Char_Move>().StraveSpeed;

    }
	
	// Update is called once per frame
	public void OverUpdate () {

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += _levelAxis.forward * Time.deltaTime * _straveSpeed;
            GetComponent<Char_Vis>().Moved();
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement += -_levelAxis.forward * Time.deltaTime * _straveSpeed;
            GetComponent<Char_Vis>().Moved();
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement += -_levelAxis.right * Time.deltaTime * _straveSpeed;
            GetComponent<Char_Vis>().Moved();
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += _levelAxis.right * Time.deltaTime * _straveSpeed;
            GetComponent<Char_Vis>().Moved();
        }

        //Look at place where the mouse is on the field

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit)){

            Vector3 pos = hit.point;

            pos.y = transform.position.y;

            transform.LookAt(pos);

        }

        if (!_characterController.isGrounded)
        {
            movement += -transform.up;
        }

        _characterController.Move(movement);

    }
}
