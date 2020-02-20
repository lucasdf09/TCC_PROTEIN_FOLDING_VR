using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class ScrollbarEventsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool gazed_at;
    private bool button_down;

    private void Awake()
    {
        gazed_at = false;
        button_down = false;
    }

    private void Update()
    {
        if (gazed_at && Input.GetButtonDown("Submit"))
        {
            Debug.Log("Gazed TRUE");
            //gazed_at = false;
            button_down = true;

            Debug.Log("Click: Pointer down!");
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        }

        else if (button_down && Input.GetButtonUp("Submit"))
        {
            Debug.Log("Button TRUE");
            //gazed_at = false;
            button_down = false;

            Debug.Log("Click: Pointer up!");
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        gazed_at = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        gazed_at = false;
        //button_down = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.beginDragHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.endDragHandler);
    }

}
