using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a coroutine to modify (adjust) the position of a game object.
/// </summary>
public class CoroutineAdjustDisplay : MonoBehaviour
{
    private GameObject target;          // Reference to the object to be adjusted

    private GameObject caller_button;   // Reference to the button that called the adjustment

    private Vector3 origin_position;    // Target position before the adjustment

    /// <summary>
    /// Starts the Adjust Position coroutine.
    /// </summary>
    /// <param name="target">Object to be adjusted.</param>
    /// <param name="caller_button">Button object reference.</param>
    public void StartAdjustPosition(GameObject target, GameObject caller_button)
    {
        this.target = target;
        this.caller_button = caller_button;
        origin_position = target.GetComponent<RectTransform>().localPosition;
        StartCoroutine("AdjustPosition");
    }

    /// <summary>
    /// Uses the joystick axes to change the position of the target object on the display.
    /// </summary>
    /// <returns>Always null.</returns>    
    private IEnumerator AdjustPosition ()
    {
        while (!Input.GetButtonDown("D"))
        {
            if(Input.GetAxis("Horizontal") != 0)
            {
                target.transform.Translate(Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                target.transform.Translate(Vector3.up * Input.GetAxisRaw("Vertical") * Time.deltaTime);
            }
            if (Input.GetAxis("Z-axis") != 0)
            {
                target.transform.Translate(Vector3.forward * Input.GetAxisRaw("Z-axis") * Time.deltaTime);
            }
            yield return null;
        }
        Debug.Log("Coroutine adjust while loop ended.");
        caller_button.GetComponent<ConfirmAdjustDisplay>().confirmPosition(origin_position);
    }
}