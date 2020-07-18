using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reset the scrollbar to the list beginning (upper position)
/// </summary>
public class ScrollbarReset : MonoBehaviour
{
    public int reset_value;

    /// <summary>
    /// Reset the scrollbar to the list beginning (upper position)
    /// </summary>
    public void resetScrollbar()
    {      
        gameObject.GetComponent<Scrollbar>().value = reset_value;
        Debug.Log("Scrollbar: " + gameObject.GetComponent<Scrollbar>().value);
    }
}
