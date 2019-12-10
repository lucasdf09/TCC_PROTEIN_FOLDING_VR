using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
