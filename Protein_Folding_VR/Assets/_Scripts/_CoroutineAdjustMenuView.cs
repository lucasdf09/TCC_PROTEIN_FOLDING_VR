using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the adjust menu view operations in a coroutine.
/// </summary>
public class CoroutineAdjustMenuView : MonoBehaviour
{
    public float angle_rate;                            // Adjust the angle rotation rate

    /// <summary>
    /// Starts the adjust menu view coroutine.
    /// </summary>
    public void StartAdjustMenuView()
    {
        StartCoroutine("AdjustMenuView");
    }

    /// <summary>
    /// Stops the adjust menu view coroutine.
    /// </summary>
    public void StopAdjustMenuView()
    {
        StopCoroutine("AdjustMenuView");
    }

    /// <summary>
    /// Adjust menu view operations coroutine.
    /// </summary>
    /// <returns>Always null.</returns>
    private IEnumerator AdjustMenuView()
    {
        while (true)
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
    }
}
