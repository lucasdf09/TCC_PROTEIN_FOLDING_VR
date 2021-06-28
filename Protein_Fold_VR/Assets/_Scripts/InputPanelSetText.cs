using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the method to set the Input Field info text.
/// </summary>
public class InputPanelSetText : MonoBehaviour
{
    [SerializeField]
    private Text input_text = default;

    /// <summary>
    /// Set the Input Field info text.
    /// </summary>
    /// <param name="text">Info text.</param>
    public void setInputText(string text)
    {
        input_text.text = text;
    }
}
