using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarReset : MonoBehaviour
{
    public void resetScrollbar()
    {      
        gameObject.GetComponent<Scrollbar>().value = 1;
        Debug.Log("Scrollbar: " + gameObject.GetComponent<Scrollbar>().value);
    }
}
