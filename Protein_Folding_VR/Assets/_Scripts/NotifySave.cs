using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows in a modal window a message about the save file status (fail or success).
/// </summary>
public class NotifySave : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject input_panel = default;           // Reference to Input Panel

    [SerializeField]
    GameObject keyboard = default;              // Reference to Keyboard Container

    [SerializeField]
    GameObject next_panel = default;            // Reference to panel to show after closing

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Closes the Input/Keyboard window and
    /// opens a modal window to show the save file status.
    /// </summary>
    /// <param name="message">Status message.</param>
    public void notifySave(string message)
    {
        input_panel.GetComponent<InputFieldReset>().resetInputField();
        keyboard.GetComponent<KeyboardReset>().resetKeyboard();       
        input_panel.SetActive(false);
        keyboard.SetActive(false);
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Return to Save Menu.
    /// </summary>
    private void okFunction()
    {
        next_panel.SetActive(true);
    }
}
