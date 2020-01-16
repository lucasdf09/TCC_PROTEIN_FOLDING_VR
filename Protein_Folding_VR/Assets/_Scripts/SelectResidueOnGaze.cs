using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Selects the residue to be manipulated using Gaze Input (Reticle Pointer)
public class SelectResidueOnGaze : MonoBehaviour
{
    public void pointerEnter()
    {
        PlayerController.target_color = gameObject.GetComponent<Renderer>().material.color;
        PlayerController.target = gameObject;
        Debug.Log("Pointer ENTER");
    }

    public void pointerExit()
    {
        if (PlayerController.select_mode)
        {
            PlayerController.target.GetComponent<Renderer>().material.color = PlayerController.target_color;
            PlayerController.target = null;
        }   
        Debug.Log("Pointer EXIT");
    }
}
