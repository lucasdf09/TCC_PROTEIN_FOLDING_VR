using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CoroutineRecenterMainView : MonoBehaviour
{
    private GameObject return_menu;                     // Reference to the menu to return in the end
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="caller_button"></param>
    public void startRecenterMainView(GameObject return_menu)
    {
        this.return_menu = return_menu;
        StartCoroutine("recenterMainView");
    }

    /// <summary>
    /// Adjust game view operations coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator recenterMainView()
    {
        //return_menu.SetActive(true);
        while (!Input.GetButtonDown("D"))
        {
            // Wait the MENU joystick button click
            yield return null;
        }
        Debug.Log("Coroutine adjust while loop ended.");

        // Use the current position and orientation to center the view
        UnityEngine.XR.InputTracking.Recenter();

        return_menu.SetActive(true);
    }
}
