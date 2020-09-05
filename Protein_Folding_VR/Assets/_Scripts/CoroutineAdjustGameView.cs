using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the adjust game view operations in a coroutine.
/// </summary>
public class CoroutineAdjustGameView : MonoBehaviour
{
    public float angle_rate;                            // Adjust the angle rotation rate

    private GameObject structure;                       // Reference to the structure

    private GameObject caller_button;                   // Reference to the button that called the adjustment

    private Quaternion previous_rotation;               // Player rotation before the adjust

    [SerializeField]
    private GameObject menu_container = default;        // Menu container object reference

    [SerializeField]
    private GameObject keyboard_container = default;    // Keyboard container object reference

    /// <summary>
    /// Starts the adjust game view coroutine.
    /// </summary>
    /// <param name="structure">Structure reference.</param>
    /// <param name="caller_button">Caller button reference.</param>
    public void StartAdjustGameView(GameObject structure, GameObject caller_button)
    {
        this.structure = structure;
        this.caller_button = caller_button;
        previous_rotation = gameObject.transform.rotation;
        StartCoroutine("adjustGameView");
    }

    /// <summary>
    /// Adjust game view operations coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator adjustGameView()
    {
        while (!Input.GetButtonDown("D"))
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                gameObject.transform.Rotate(Vector3.up, angle_rate * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                gameObject.transform.Rotate(Vector3.left, angle_rate * Input.GetAxisRaw("Vertical") * Time.deltaTime);
            }
            if (Input.GetAxis("Z-axis") != 0)
            {
                gameObject.transform.Rotate(Vector3.forward, angle_rate * Input.GetAxisRaw("Z-axis") * Time.deltaTime);
            }
            yield return null;
        }
        Debug.Log("Coroutine adjust while loop ended.");
        
        // Hide the strucure from player view 
        structure.SetActive(false);
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

        caller_button.GetComponent<ConfirmAdjustGameView>().confirmAdjustGameView(previous_rotation);
    }
}
