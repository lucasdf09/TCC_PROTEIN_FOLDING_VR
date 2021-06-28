using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Raise the "Click" EventSystem when the player is looking at the game toggle button (with the Reticle Pointer - RP)
/// and press the "C" joystick button.
/// </summary>
public class ToggleJoystickClick : MonoBehaviour
{
    private bool gazed_at = false;      // Flag to track the RP over the toggle button
    private bool button_down = false;   // Flag to store the toggle button press

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        // Condition to emulate a click down in the VR button
        // The button is gazed and the action button is pressed
        if (gazed_at && Input.GetButtonDown("Submit"))
        {
            //Debug.Log("Gazed TRUE");
            button_down = true;
            //Debug.Log("Click: Pointer down!");
            // Rise a Pointer Click Down event, that can be catched by the Event Trigger
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        }

        // Condition to emulate a click up in the VR button
        // The button continues to be gazed and the action button (once pressed) is released
        else if (gazed_at && button_down && Input.GetButtonUp("Submit"))
        {
            //Debug.Log("Button TRUE");
            //gazed_at = false;
            button_down = false;
            //Debug.Log("Click: Pointer up!");
            // Rise a Pointer Click Up event, that can be catched by the Event Trigger
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }

#elif UNITY_ANDROID
        // Condition to emulate a click down in the VR button
        // The button is gazed and the action button is pressed
        if (gazed_at && Input.GetButtonDown("C"))
        {
            //Debug.Log("Gazed TRUE");
            button_down = true;
            //Debug.Log("Click: Pointer down!");
            // Rise a Pointer Click Down event, that can be catched by the Event Trigger
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        }

        // Condition to emulate a click up in the VR button
        // The button continues to be gazed and the action button (once pressed) is released
        else if (gazed_at && button_down && Input.GetButtonUp("C"))
        {
            //Debug.Log("Button TRUE");
            //gazed_at = false;
            button_down = false;
            //Debug.Log("Click: Pointer up!");
            // Rise a Pointer Click Up event, that can be catched by the Event Trigger
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }

#endif

    }

    /// <summary>
    /// Rises gazed_at to true
    /// when the Reticle Pointer is over.
    /// </summary>
    public void pointerEnter()
    {
        gazed_at = true;
    }

    /// <summary>
    /// Rises gazed_at and button_down to false
    /// when the Reticle Pointer is out.
    /// </summary>
    public void pointerExit()
    {
        gazed_at = false;
        button_down = false;
    }
}
