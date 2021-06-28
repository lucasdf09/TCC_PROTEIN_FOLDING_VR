using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements joystick input to controls the scrollbar slide in vertical axis.
/// </summary>
public class ScrollbarJoystickArrows : MonoBehaviour
{
    public string axis;
    public float step;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw(axis) != 0)
        {
            gameObject.GetComponent<Scrollbar>().value = Mathf.Clamp(gameObject.GetComponent<Scrollbar>().value + (Input.GetAxisRaw(axis) * step * Time.deltaTime), 0, 1);
        }
    }
}
