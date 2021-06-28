using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the recenter operation in game scene.
/// </summary>
public class CoroutineRecenterGameView : MonoBehaviour
{
    private GameObject return_menu;                     // Reference to the menu to return after the recenter

    [SerializeField]
    private GameObject structure = default;             // Reference to the structure object

    [SerializeField]
    private GameObject menu_container = default;        // Menu container object reference

    [SerializeField]
    private GameObject keyboard_container = default;    // Keyboard container object reference

    /// <summary>
    /// Starts the recenterGameView coroutine.
    /// </summary>
    /// <param name="return_menu">Reference to the menu to return after the recenter.</param>
    public void startRecenterGameView(GameObject return_menu)
    {
        this.return_menu = return_menu;
        StartCoroutine("recenterGameView");
    }

    /// <summary>
    /// Waits until the D button press to recenter the game viewpoint and adjust the menu objects position and orientation.
    /// </summary>
    /// <returns>Always null.</returns>
    private IEnumerator recenterGameView()
    {
        // Show the structure
        structure.SetActive(true);

        while (!Input.GetButtonDown("D"))
        {
            // Wait the MENU joystick button click
            yield return null;
        }
        Debug.Log("Coroutine recenterGameView loop ended.");

        // Hide the strucure from player view 
        structure.SetActive(false);

        // Use the current position and orientation to recenter the view
        UnityEngine.XR.InputTracking.Recenter();

        // Set the Game Menu in front of the player view
        menu_container.transform.position = gameObject.transform.position + gameObject.transform.forward * 2;
        menu_container.transform.rotation = gameObject.transform.rotation;
        // Set the Keyboard in front of the player view
        keyboard_container.transform.position = gameObject.transform.position + gameObject.transform.forward * 2;
        keyboard_container.transform.rotation = gameObject.transform.rotation;
        // Rotate the Keyboard to a better position for the player interaction
        keyboard_container.transform.Rotate(30, 0, 0);

        // Get the camera positioning to restore it when load a game after a saving
        PlayerController.camera_position = gameObject.transform.localPosition;
        PlayerController.camera_rotation = gameObject.transform.localRotation;

        // Show the menu panel
        return_menu.SetActive(true);
    }
}
