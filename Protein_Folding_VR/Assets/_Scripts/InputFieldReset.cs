using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the method to clear the input field text.
/// </summary>
public class InputFieldReset : MonoBehaviour
{
    [SerializeField]
    private InputField text_input = default;

    /// <summary>
    /// Clears the input field.
    /// </summary>
    public void resetInputField()
    {
        text_input.text = "";
    }
}
