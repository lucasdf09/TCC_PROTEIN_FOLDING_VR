using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Selects the residue to be manipulated using Gaze Input (Reticle Pointer).
/// </summary>
public class SelectResidueOnGaze : MonoBehaviour
{
    /// <summary>
    /// Set the attached color and game object as
    /// target_color and target reference in PLayerController script.
    /// </summary>
    public void pointerEnter()
    {
        PlayerController.target_color = gameObject.GetComponent<Renderer>().material.color;
        PlayerController.target = gameObject;
        Debug.Log("Pointer ENTER");
    }

    /// <summary>
    /// Clear the target_color and target reference in PlayerController Script.
    /// </summary>
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
