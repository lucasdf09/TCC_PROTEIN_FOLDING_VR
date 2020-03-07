using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardBackspaceInput : MonoBehaviour
{
    private InputField text_input;
    private InputFieldCaret text_caret;

    private void Start()
    {
        GameObject input_field = GameObject.FindGameObjectWithTag("KeyboardInputField");
        text_input = input_field.GetComponent<InputField>();
        text_caret = input_field.GetComponent<InputFieldCaret>();
    }

    // Remove the last character of the Input Field (IF) text string.
    public void sendBackspace()
    {
        if(text_input.text.Length > 0)
        {
            Debug.Log("Character: " + gameObject.GetComponentInChildren<Text>().text);
            text_input.text = text_input.text.Remove(text_input.text.Length - 1);
            text_caret.SetCarretVisible(text_input.text.Length);
        }           
    }


}