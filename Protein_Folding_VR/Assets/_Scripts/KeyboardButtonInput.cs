using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonInput : MonoBehaviour
{
    private InputField text_input;
    private InputFieldCaret text_caret;

    private void Start()
    {
        GameObject input_field = GameObject.FindGameObjectWithTag("KeyboardInputField");
        text_input = input_field.GetComponent<InputField>();
        text_caret = input_field.GetComponent<InputFieldCaret>();
    }

    // Set the Input Field (IF) text as the actual IF text, plus, the keyboard button text (character).
    public void sendCharacter()
    {
        Debug.Log("Character: " + gameObject.GetComponentInChildren<Text>().text);

        text_input.text += gameObject.GetComponentInChildren<Text>().text;
        text_caret.SetCarretVisible(text_input.text.Length);
    }
}
