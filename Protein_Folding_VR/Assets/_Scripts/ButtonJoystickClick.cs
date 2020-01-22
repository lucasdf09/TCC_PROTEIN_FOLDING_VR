using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Raise the "Click" EventSystem when the player is looking at the game button
// and press the "Fire1" joystick button
public class ButtonJoystickClick : MonoBehaviour
{

    private bool gazed_at = false;

    // Update is called once per frame
    void Update()
    {
        if (gazed_at && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Gazed TRUE");
            gazed_at = false;
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }

    public void pointerEnter()
    {
        gazed_at = true;
    }

    public void pointerExit()
    {
        gazed_at = false;
    }

}
