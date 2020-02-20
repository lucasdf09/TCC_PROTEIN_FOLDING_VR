using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarJoystickArrows : MonoBehaviour
{
    public float step;

    private Scrollbar scroll;
    
    // Start is called before the first frame update
    void Start()
    {
        scroll = gameObject.GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            scroll.value = Mathf.Clamp(scroll.value + (Input.GetAxisRaw("Vertical") * step * Time.deltaTime), 0, 1);
        }
    }
}
