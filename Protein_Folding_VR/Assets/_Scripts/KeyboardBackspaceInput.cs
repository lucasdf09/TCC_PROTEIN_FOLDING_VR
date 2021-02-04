using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the backspace keyboard input.
/// </summary>
public class KeyboardBackspaceInput : MonoBehaviour
{
    private InputField text_input;          // InputField text field reference
    private InputFieldCaret text_caret;     // InputField caret position handler

    private void Start()
    {
        GameObject input_field = GameObject.FindGameObjectWithTag("KeyboardInputField");
        text_input = input_field.GetComponent<InputField>();
        text_caret = input_field.GetComponent<InputFieldCaret>();
    }

    /// <summary>
    /// Remove the last character of the Input Field text string.
    /// </summary>
    public void sendBackspace()
    {
        if(text_input.text.Length > 0)
        {
            //Debug.Log("Character: " + gameObject.GetComponentInChildren<Text>().text);
            // Remotion of the last character
            text_input.text = text_input.text.Remove(text_input.text.Length - 1);
            // Caret position update
            text_caret.SetCarretVisible(text_input.text.Length);
        }           
    }
}