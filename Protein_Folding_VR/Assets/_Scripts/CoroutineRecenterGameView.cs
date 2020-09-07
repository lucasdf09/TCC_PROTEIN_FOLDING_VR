using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CoroutineRecenterGameView : MonoBehaviour
{
    private GameObject return_menu;                     // Reference to the menu to return in the end

    [SerializeField]
    private GameObject structure = default;             // Reference to the structure

    [SerializeField]
    private GameObject menu_container = default;        // Menu container object reference

    [SerializeField]
    private GameObject keyboard_container = default;    // Keyboard container object reference


    public void startRecenterGameView(GameObject return_menu)
    {
        this.return_menu = return_menu;
        StartCoroutine("recenterGameView");
    }


    private IEnumerator recenterGameView()
    {
        // Show the structure to player view
        structure.SetActive(true);

        while (!Input.GetButtonDown("D"))
        {
            // Wait the MENU joystick button click
            yield return null;
        }
        Debug.Log("Coroutine adjust while loop ended.");

        // Hide the strucure from player view 
        structure.SetActive(false);

        // Use the current position and orientation to center the view
        UnityEngine.XR.InputTracking.Recenter();

        /*
        // Set the Game Menu in front of the player view using the camera as reference
        menu_container.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        menu_container.transform.rotation = Camera.main.transform.rotation;
        //menu_container.SetActive(true);
        // Set the Keyboard in front of the player view using the camera as reference
        keyboard_container.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        keyboard_container.transform.rotation = Camera.main.transform.rotation;
        // Rotate the Keyboard to a better position for the player interaction
        keyboard_container.transform.Rotate(30, 0, 0);

        // Get the camera positioning to restore it when load a game
        //camera_position = Camera.main.transform.localPosition;
        //camera_rotation = Camera.main.transform.localRotation;
        PlayerController.camera_position = Camera.main.transform.position;
        PlayerController.camera_rotation = Camera.main.transform.rotation;
        */

        return_menu.SetActive(true);
    }
}
