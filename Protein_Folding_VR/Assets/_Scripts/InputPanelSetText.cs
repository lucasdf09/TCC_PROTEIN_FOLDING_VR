using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanelSetText : MonoBehaviour
{
    [SerializeField]
    private Text input_text = default;

    public void setInputText(string text)
    {
        input_text.text = text;
    }
}
