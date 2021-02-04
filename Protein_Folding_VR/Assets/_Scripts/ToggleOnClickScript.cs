using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Triggers a click event in the toggle button attached to the script game object.
/// </summary>
public class ToggleOnClickScript : MonoBehaviour
{
    /// <summary>
    /// Invokes an onClick event.
    /// </summary>
    public void togglenOnClick()
    {
        gameObject.GetComponent<Toggle>().isOn = !gameObject.GetComponent<Toggle>().isOn;
    }
}
