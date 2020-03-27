using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Triggers an click event in the button attached to the script game object
public class ButtonOnClickScript : MonoBehaviour
{
    public void buttonOnClick()
    {
        gameObject.GetComponent<Button>().onClick.Invoke();
    }
}
