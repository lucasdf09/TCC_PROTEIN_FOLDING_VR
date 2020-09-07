using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Triggers an click event in the button attached to the script game object.
/// </summary>
public class ButtonOnClickScript : MonoBehaviour
{
    /// <summary>
    /// Invokes an onClick event.
    /// </summary>
    public void buttonOnClick()
    {
        gameObject.GetComponent<Button>().onClick.Invoke();
    }
}
