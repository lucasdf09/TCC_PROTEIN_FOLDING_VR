using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements a key button character input.
/// </summary>
public class KeyboardButtonInput : MonoBehaviour
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
    /// Insert the keyboard button text (character) in the InputField text.
    /// </summary>
    public void sendCharacter()
    {
        Debug.Log("Character: " + gameObject.GetComponentInChildren<Text>().text);
        // Character insertion
        text_input.text += gameObject.GetComponentInChildren<Text>().text;
        // Caret position update
        text_caret.SetCarretVisible(text_input.text.Length);
    }
}
