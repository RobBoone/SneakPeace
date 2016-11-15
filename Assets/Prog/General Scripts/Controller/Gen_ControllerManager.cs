#define WindowsBuild

#if WindowsBuild
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gen_ControllerManager : MonoBehaviour 
{ 
    //Number of controllers to support
   public int ControllerCount = 4;

    // controller istance
    private List<Gen_ControllerInput> _Controllers;

    //singleton instance
    private static Gen_ControllerManager _Singleton; 

    //Initialize on 'Awake'
    void Awake ()
    {
        // Found a duplicate instance of this class, destroy it!
        if (_Singleton != null && _Singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Create singleton instance
            _Singleton = this;
            DontDestroyOnLoad(this.gameObject);

            // Lock GamepadCount to supported range
            ControllerCount = Mathf.Clamp(ControllerCount, 1, 4);

            _Controllers = new List<Gen_ControllerInput>();

            // Create specified number of gamepad instances
            for (int i = 0; i < ControllerCount; ++i)
            {
                _Controllers.Add(new Gen_ControllerInput(i + 1));
            }
        }
    }

    // Return instance
    public static Gen_ControllerManager Instance
    {
        get
        {
            if (_Singleton == null)
            {
                Debug.LogError("[ControllerManager]: Instance does not exist!");
                return null;
            }

            return _Singleton;
        }
    }

    void Update ()
    {
        for (int i = 0; i < _Controllers.Count; ++i)
        {
            _Controllers[i].Update();
        }
    }

    // Refresh gamepad states for next update
    public void Refresh()
    {
        for (int i = 0; i < _Controllers.Count; ++i)
            _Controllers[i].Refresh();
    }

    // Return specified gamepad
    // (Pass index of desired gamepad, eg. 1)
    public Gen_ControllerInput GetController(int index)
    {
        for (int i = 0; i < _Controllers.Count;)
        {
            // Indexes match, return this gamepad
            if (_Controllers[i].Index == (index - 1))
                return _Controllers[i];
            else
                ++i;
        }

        Debug.LogError("[ControllerManager]: " + index + " is not a valid controller index!");

        return null;
    }

    // Return number of connected gamepads
    public int ConnectedTotal()
    {
        int total = 0;

        for (int i = 0; i < _Controllers.Count; ++i)
        {
            if (_Controllers[i].IsConnected)
                total++;
        }

        return total;
    }

    // Check across all gamepads for button press.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonAny(string button)
    {
        for (int i = 0; i < _Controllers.Count; ++i)
        {
            // Gamepad meets both conditions
            if (_Controllers[i].IsConnected && _Controllers[i].GetButton(button))
                return true;
        }

        return false;
    }

    // Check across all gamepads for button press - CURRENT frame.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonDownAny(string button)
    {
        for (int i = 0; i < _Controllers.Count; ++i)
        {
            // Gamepad meets both conditions
            if (_Controllers[i].IsConnected && _Controllers[i].GetButtonDown(button))
                return true;
        }

        return false;
    }

    // Removes rumble accross all gamepads
    public void RumbleAll()
    {
        for (int i = 0; i < _Controllers.Count; ++i)
        {
            // Gamepad is connected
            if (_Controllers[i].IsConnected)
                _Controllers[i].AddRumble(1.0f, new Vector2(0.5f, 0.5f), 0.5f);
        }
    }

// Removes rumble accross all gamepads
public void RemoveRumbleAll()
    {
        for (int i = 0; i < _Controllers.Count; ++i)
        {
            // Gamepad is connected
            if (_Controllers[i].IsConnected)
                _Controllers[i].ClearRumble();
        }
    }
}
#endif