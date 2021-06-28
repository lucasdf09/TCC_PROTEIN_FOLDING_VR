using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the joystick check and modal panel call, if necessary.
/// </summary>
public class VerifyJoystick : MonoBehaviour
{
    [SerializeField]
    GameObject menu_panel = default;            // Reference to panel to return

    private void Start()
    {
        //Get Joystick Names
        string[] joysticks = Input.GetJoystickNames();

        //Check whether array contains anything
        if (joysticks.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < joysticks.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(joysticks[i]))
                {
                    //Not empty, controller joysticks[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + joysticks[i]);
                }
                else
                {
                    //If it is empty, controller [i] is disconnected
                    //where [i] indicates the controller number
                     Debug.Log("Controller: " + i + " is disconnected.");
#if UNITY_EDITOR
                    Debug.Log("EDITOR: Joystick absence!");
#elif UNITY_ANDROID
                    menu_panel.SetActive(false);
                    gameObject.GetComponent<NotifyJoystick>().notifyJoystickAbsence();
#endif
                }
            }
        }
        else
        {
            Debug.Log("No joystick found.");
#if UNITY_EDITOR
            Debug.Log("EDITOR: Joystick absence!");
#elif UNITY_ANDROID
            menu_panel.SetActive(false);
            gameObject.GetComponent<NotifyJoystick>().notifyJoystickAbsence();
#endif 
        }
    }
}
