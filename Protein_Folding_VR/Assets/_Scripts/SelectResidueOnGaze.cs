using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectResidueOnGaze : MonoBehaviour
{
    private bool gazed_at = false;
    //private Color res_color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gazed_at && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Gazed TRUE");
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            gazed_at = false;
        }
    }

    public void pointerClick()
    {
        Debug.Log("CLICK key was pressed");
        PlayerController.color_aux = PlayerController.target_color;
        PlayerController.color_aux.a = 0.1f;
        PlayerController.select_mode = false;
        PlayerController.move_mode = true;
        Debug.Log("Select mode: " + PlayerController.select_mode);
        Debug.Log("Move mode: " + PlayerController.move_mode);
        PlayerController.movement = PlayerController.target.GetComponent<Rigidbody>().transform.position;

        reticle_pointer.SetActive(false);
    }

    public void pointerEnter()
    {
        gazed_at = true;
        //res_color = gameObject.GetComponent<Renderer>().material.color;
        PlayerController.target_color = gameObject.GetComponent<Renderer>().material.color;
        PlayerController.target = gameObject;
    }

    public void pointerExit()
    {
        gazed_at = false;
        PlayerController.target.GetComponent<Renderer>().material.color = PlayerController.target_color;
        PlayerController.target = null;
    }
}
