using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldCaret : MonoBehaviour
{
    // Code source: https://forum.unity.com/threads/howto-inputfield-always-show-caret.424556/ 
    public void SetCarretVisible(int pos)
    {
        InputField inputField = gameObject.GetComponent<InputField>();
        inputField.caretPosition = pos; // desired cursor position

        inputField.GetType().GetField("m_AllowInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inputField, true);
        inputField.GetType().InvokeMember("SetCaretVisible", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, inputField, null);
    }
}
