using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the method to reset the keyboard to the standard layout.
/// </summary>
public class KeyboardReset : MonoBehaviour
{
    [SerializeField]
    private GameObject standard_keyboard = default;

    [SerializeField]
    private GameObject capital_keyboard = default;

    [SerializeField]
    private GameObject numerical_keyboard = default;

    /// <summary>
    /// Reset the keyboard to the standard layout.
    /// </summary>
    public void resetKeyboard()
    {
        standard_keyboard.SetActive(true);
        capital_keyboard.SetActive(false);
        numerical_keyboard.SetActive(false);
    }
}
