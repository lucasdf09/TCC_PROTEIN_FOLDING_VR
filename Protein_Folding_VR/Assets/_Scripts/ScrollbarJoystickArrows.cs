using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarJoystickArrows : MonoBehaviour
{
    public float step;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            gameObject.GetComponent<Scrollbar>().value = Mathf.Clamp(gameObject.GetComponent<Scrollbar>().value + (Input.GetAxisRaw("Vertical") * step * Time.deltaTime), 0, 1);
        }
    }
}
