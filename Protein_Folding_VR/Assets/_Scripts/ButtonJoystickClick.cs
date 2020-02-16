using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Raise the "Click" EventSystem when the player is looking at the game button
// and press the "Fire1" joystick button
public class ButtonJoystickClick : MonoBehaviour
{
    private bool gazed_at = false;
    private bool button_down = false;

    // Update is called once per frame
    void Update()
    {
        if (gazed_at && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Gazed TRUE");
            //gazed_at = false;
            button_down = true;
#if UNITY_EDITOR

#elif UNITY_ANDROID
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
#endif
        }

        else if (gazed_at && button_down && Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Button TRUE");
            gazed_at = false;
            button_down = false;
#if UNITY_EDITOR

#elif UNITY_ANDROID
            Debug.Log("pointer up!");
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
#endif
        }
    }

    public void pointerEnter()
    {
        gazed_at = true;
    }

    public void pointerExit()
    {
        gazed_at = false;
        button_down = false;
    }
}
