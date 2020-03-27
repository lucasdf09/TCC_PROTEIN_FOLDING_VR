using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Clear the input field text.
// Caret treatment?
public class InputFieldReset : MonoBehaviour
{
    [SerializeField]
    private InputField text_input = default;

    public void resetInputField()
    {
        text_input.text = "";
    }
}
