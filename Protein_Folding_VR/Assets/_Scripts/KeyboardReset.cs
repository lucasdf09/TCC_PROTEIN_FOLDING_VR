using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reset the keyboard to the standard layout
public class KeyboardReset : MonoBehaviour
{
    [SerializeField]
    private GameObject standard_keyboard;

    [SerializeField]
    private GameObject capital_keyboard;

    [SerializeField]
    private GameObject numerical_keyboard;

    public void resetKeyboard()
    {
        standard_keyboard.SetActive(true);
        capital_keyboard.SetActive(false);
        numerical_keyboard.SetActive(false);
    }
}
