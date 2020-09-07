using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements joystick input to controls the scrollbar slide in horizontal axis.
/// </summary>
public class ScrollbarJoystickHorizontal : MonoBehaviour
{
    public float step;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            gameObject.GetComponent<Scrollbar>().value = Mathf.Clamp(gameObject.GetComponent<Scrollbar>().value + (Input.GetAxisRaw("Horizontal") * step * Time.deltaTime), 0, 1);
        }
    }
}
