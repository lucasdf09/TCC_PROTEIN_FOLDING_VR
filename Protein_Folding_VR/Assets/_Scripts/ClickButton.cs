using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickButton : MonoBehaviour
{

    private bool gazed_at;

    // Start is called before the first frame update
    void Start()
    {
        gazed_at = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gazed_at && Input.GetButton("B"))
        {
            Debug.Log("Gazed TRUE");
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            gazed_at = false;
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
