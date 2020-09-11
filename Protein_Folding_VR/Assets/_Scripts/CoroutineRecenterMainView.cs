using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the recenter operation in main menu scene.
/// </summary>
public class CoroutineRecenterMainView : MonoBehaviour
{
    private GameObject return_menu;             // Reference to the menu to return after the recenter

    /// <summary>
    /// Starts the recenterMainView coroutine.
    /// </summary>
    /// <param name="return_menu">Reference to the menu to return after the recenter.</param>
    public void startRecenterMainView(GameObject return_menu)
    {
        this.return_menu = return_menu;
        StartCoroutine("recenterMainView");
    }

    /// <summary>
    /// Waits until the MENU button press to recenter the game viewpoint.
    /// </summary>
    /// <returns>Always null.</returns>
    private IEnumerator recenterMainView()
    {
        //return_menu.SetActive(true);
        while (!Input.GetButtonDown("D"))
        {
            // Wait the MENU joystick button click
            yield return null;
        }
        Debug.Log("Coroutine recenterMainView loop ended.");

        // Use the current position and orientation to recenter the view
        UnityEngine.XR.InputTracking.Recenter();

        // Show the menu panel
        return_menu.SetActive(true);
    }
}
