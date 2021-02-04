using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the method to turns input filed caret always on.
/// </summary>
public class InputFieldCaret : MonoBehaviour
{
    /// <summary>
    /// turns input filed caret always on.
    /// Code source: https://forum.unity.com/threads/howto-inputfield-always-show-caret.424556/ 
    /// </summary>
    /// <param name="pos">Caret position.</param>
    public void SetCarretVisible(int pos)
    {
        InputField inputField = gameObject.GetComponent<InputField>();
        inputField.caretPosition = pos; // desired cursor position

        inputField.GetType().GetField("m_AllowInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inputField, true);
        inputField.GetType().InvokeMember("SetCaretVisible", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, inputField, null);
    }
}
