#define WindowsBuild

#if WindowsBuild
using UnityEngine;
using XInputDotNetPure;

using System.Collections.Generic;

/*
//
//Xbox 360 Gamepad Input script
// This script is for handling all the input of the controllers/gamepads
// with DirectXInput.
//
*/

//Stores states of a single gamepad button
public struct xButton
{
    public ButtonState PrevState;
    public ButtonState State;
}

//Stores states of a single gamepad trigger
public struct TriggerState
{
    public float PrevValue;
    public float CurrentValue;
}

//Rumble (vibration) event
class xRumble
{
    // Rumble timer & the face-out (in seconds)
    public float Timer;
    public float FadeTime;

    // Rumble Intensity
    public Vector2 Power;

    // Decrease timer
    public void Update()
    {
        this.Timer -= Time.deltaTime;
    }
}


// Xbox 360 Gamepad class
public class Gen_ControllerInput
{
    //Previous and current gamepad state
    private GamePadState _PrevState;
    private GamePadState _State;

    //Numeric gamepad index (1,2,3 or 4) and XInput 'Player' index
    private int _GamepadIndex;
    private PlayerIndex _PlayerIndex;

    //Stores rumble events
    private List<xRumble> _RumbleEvents;

    //Button input map 
    private Dictionary<string, xButton> _InputMap;

    // States for all buttons/inputs supported
    private xButton _A, _B, _X, _Y;
    private xButton _DPadUp, _DPadDown, _DPadLeft, _DPadRight;
    private xButton _Back, _Start;
    private xButton _L3, _R3;
    private xButton _LB, _RB;
    private TriggerState _LT, _RT;

    // constructor
    public Gen_ControllerInput(int index)
    {
        //set gamepad index
        _GamepadIndex = index - 1;
        _PlayerIndex = (PlayerIndex)_GamepadIndex;

        //Create rumble container and input map
        _RumbleEvents = new List<xRumble>();
        _InputMap = new Dictionary<string, xButton>();
    }

    //update gamepad state
    public void Update ()
    {
        //get current state
        _State = GamePad.GetState(_PlayerIndex);

        //check gamepad is connected
        if(_State.IsConnected)
        {
            _A.State = _State.Buttons.A;
            _B.State = _State.Buttons.B;
            _X.State = _State.Buttons.X;
            _Y.State = _State.Buttons.Y;

            _DPadUp.State = _State.DPad.Up;
            _DPadDown.State = _State.DPad.Down;
            _DPadLeft.State = _State.DPad.Left;
            _DPadRight.State = _State.DPad.Right;

            _Back.State = _State.Buttons.Back;
            _Start.State = _State.Buttons.Start;
            _L3.State = _State.Buttons.LeftStick;
            _R3.State = _State.Buttons.RightStick;
            _LB.State = _State.Buttons.LeftShoulder;
            _RB.State = _State.Buttons.RightShoulder;

            // Read trigger values into trigger states
            _LT.CurrentValue = _State.Triggers.Left;
            _RT.CurrentValue = _State.Triggers.Right;

            //update inputMap dictionary
            UpdateInputMap();

            // Update rumble event(s)
            HandleRumble();  
        }
	}

    // Refresh previous gamepad state
    public void Refresh()
    {
        // This 'saves' the current state for next update
        _PrevState = _State;

        // Check gamepad is connected
        if (_State.IsConnected)
        {
            _A.PrevState = _PrevState.Buttons.A;
            _B.PrevState = _PrevState.Buttons.B;
            _X.PrevState = _PrevState.Buttons.X;
            _Y.PrevState = _PrevState.Buttons.Y;

            _DPadUp.PrevState = _PrevState.DPad.Up;
            _DPadDown.PrevState = _PrevState.DPad.Down;
            _DPadLeft.PrevState = _PrevState.DPad.Left;
            _DPadRight.PrevState = _PrevState.DPad.Right;

            _Back.PrevState = _PrevState.Buttons.Back;
            _Start.PrevState = _PrevState.Buttons.Start;
            _L3.PrevState = _PrevState.Buttons.LeftStick;
            _R3.PrevState = _PrevState.Buttons.RightStick;
            _LB.PrevState = _PrevState.Buttons.LeftShoulder;
            _RB.PrevState = _PrevState.Buttons.RightShoulder;

            // Read previous trigger values into trigger states
            _LT.PrevValue = _PrevState.Triggers.Left;
            _RT.PrevValue = _PrevState.Triggers.Right;

            //update inputMap dictionary
            UpdateInputMap();
        }
    }

    
    // Update input map
    void UpdateInputMap()
    {
        _InputMap["A"] = _A;
        _InputMap["B"] = _B;
        _InputMap["X"] = _X;
        _InputMap["Y"] = _Y;

        _InputMap["DPad_Up"] = _DPadUp;
        _InputMap["DPad_Down"] = _DPadDown;
        _InputMap["DPad_Left"] = _DPadLeft;
        _InputMap["DPad_Right"] = _DPadRight;

        _InputMap["Back"] = _Back;
        _InputMap["Start"] = _Start;

        // Thumbstick buttons
        _InputMap["L3"] = _L3;
        _InputMap["R3"] = _R3;

        // Shoulder ('bumper') buttons
        _InputMap["LB"] = _LB;
        _InputMap["RB"] = _RB;
    }

    // Return button state
    public bool GetButton(string button)
    {
        //Debug.Log("Controller " + _GamepadIndex + " Button " + button);
        return _InputMap[button].State == ButtonState.Pressed ? true : false;
    }

    // Return button state - on CURRENT frame
    public bool GetButtonDown(string button)
    {
        return (_InputMap[button].PrevState == ButtonState.Released && _InputMap[button].State == ButtonState.Pressed) ? true : false;
    }

    // Return button state - on CURRENT frame
    public bool GetButtonUp(string button)
    {
        return (_InputMap[button].PrevState == ButtonState.Pressed && _InputMap[button].State == ButtonState.Released) ? true : false;
    }

    // Update and apply rumble events
    void HandleRumble()
    {
        // Ignore if there are no events
        if (_RumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(0, 0);

            for (int i = 0; i < _RumbleEvents.Count; ++i)
            {
                // Update current event
                _RumbleEvents[i].Update();

                if (_RumbleEvents[i].Timer > 0)
                {
                    // Calculate current power
                    float timeLeft = Mathf.Clamp(_RumbleEvents[i].Timer / _RumbleEvents[i].FadeTime, 0f, 1f);
                    currentPower = new Vector2(Mathf.Max(_RumbleEvents[i].Power.x * timeLeft, currentPower.x),
                                               Mathf.Max(_RumbleEvents[i].Power.y * timeLeft, currentPower.y));

                    // Apply vibration to gamepad motors
                    GamePad.SetVibration(_PlayerIndex, currentPower.x, currentPower.y);
                }
                else
                {
                    // Remove expired event
                    _RumbleEvents.Remove(_RumbleEvents[i]);
                    GamePad.SetVibration(_PlayerIndex, 0, 0);
                }
            }
        }
    }

    // Add a rumble event to the gamepad
    public void AddRumble(float timer, Vector2 power, float fadeTime)
    {
        xRumble rumble = new xRumble();

        rumble.Timer = timer;
        rumble.Power = power;
        rumble.FadeTime = fadeTime;

        _RumbleEvents.Add(rumble);
    }

    //Remove all rumble of gamepad
    public void ClearRumble()
    {
        _RumbleEvents.Clear();
        GamePad.SetVibration(_PlayerIndex, 0, 0);
    }


    // Return numeric gamepad index
    public int Index { get { return _GamepadIndex; } }

    // Return gamepad connection state
    public bool IsConnected { get { return _State.IsConnected; } }

    // Return axes of left thumbstick
    public GamePadThumbSticks.StickValue GetStick_L() { return _State.ThumbSticks.Left; }

    // Return axes of right thumbstick
    public GamePadThumbSticks.StickValue GetStick_R() { return _State.ThumbSticks.Right; }

    // Return axis of left trigger
    public float GetTrigger_L() { return _State.Triggers.Left; }

    // Return axis of right trigger
    public float GetTrigger_R() { return _State.Triggers.Right; }

    // Check if left trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_L()
    {
        return (_LT.PrevValue == 0f && _LT.CurrentValue >= 0.1f) ? true : false;
    }

    // Check if right trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_R()
    {
        return (_RT.PrevValue == 0f && _RT.CurrentValue >= 0.1f) ? true : false;
    }
}
#endif