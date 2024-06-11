using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static string[] ps4BoolNames = { "Jump", "Dash", "Back", "Pause", "SwapAbilities", "Attack", "Parry" };

    private static string[] ps4FloatNames = { "LeftHorizontal", "LeftVertical", "R2", "L2", "HorizontalArrows", "VerticalArrows", "RightVertical" };

    private static string[] xBoxBoolNames = { "JumpXbox", "Dash", "BackXbox", "PauseXbox", "SwapAbilities", "XboxAttack", "Parry" }; //Done

    private static string[] xBoxFloatNames = { "LeftHorizontal", "LeftVertical", "R2Xbox", "L2Xbox", "HorizontalArrowsXbox", "VerticalArrowsXbox", "RightVerticalXbox" };


    public static int DetectController()
    {
        string[] joystickNames = Input.GetJoystickNames();
        
        foreach (string joystickName in joystickNames)
        {
            //Debug.Log(joystickName);

            if (joystickName.Length > 0)
            {
                if (joystickName == "Wireless Controller")
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        return 0;
    }
    // Start is called before the first frame update
    public static bool GetButtonDown(string name)
    {
        switch (DetectController())
        {
            case 1:

                for (int i = 0; i < 7; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {
                        return Input.GetButtonDown(ps4BoolNames[i]);
                    }
                }
                return false;

            case 2:

                for (int i = 0; i < 7; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {
                        //Debug.Log("PS4 name: " + ps4BoolNames[i] + "  Xbox name: " + xBoxBoolNames[i]);

                        return Input.GetButtonDown(xBoxBoolNames[i]);
                    }
                }
                return false;
                
            default:return false;
        }
    }
    public static bool GetButton(string name)
    {
        switch (DetectController())
        {
            case 1:

                for (int i = 0; i < ps4BoolNames[i].Length; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {
                        return Input.GetButton(ps4BoolNames[i]);
                    }
                }
                return false;

            case 2:

                for (int i = 0; i < xBoxBoolNames[i].Length; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {

                        //Debug.Log("PS4 name: " + ps4BoolNames[i] + "  Xbox name: " + xBoxBoolNames[i]);

                        return Input.GetButton(xBoxBoolNames[i]);
                    }
                }

                return false;
                
            default:return false;
        }
    }
    public static bool GetButtonUp(string name)
    {
        switch (DetectController())
        {
            case 1:

                for (int i = 0; i < ps4BoolNames[i].Length; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {
                        return Input.GetButtonUp(ps4BoolNames[i]);
                    }
                }
                return false;

            case 2:

                for (int i = 0; i < xBoxBoolNames[i].Length; i++)
                {
                    if (ps4BoolNames[i] == name)
                    {
                       // Debug.Log("PS4 name: " + ps4BoolNames[i] + "  Xbox name: " + xBoxBoolNames[i]);
                        return Input.GetButtonUp(xBoxBoolNames[i]);
                    }
                }

                return false;
                
            default:return false;
        }
    }
    public static float GetAxis(string name)
    {
        switch (DetectController())
        {

            case 1:

                for (int i = 0; i < 7; i++)
                {
                    if (ps4FloatNames[i] == name)
                    {
                        return Input.GetAxis(ps4FloatNames[i]);
                    }
                }
                //PS
                return 0;

            case 2:

                for (int i = 0; i < 7; i++)
                {
                    if (ps4FloatNames[i] == name)
                    {
                        return Input.GetAxis(xBoxFloatNames[i]);
                    }
                }

                //Same with different string[]
                //XBox
                return 0;

            default:return 0;
        }

        
    }
}
