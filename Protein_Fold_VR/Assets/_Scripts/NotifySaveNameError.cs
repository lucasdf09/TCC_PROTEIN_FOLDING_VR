using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows in a modal window a message about a save name error.
/// </summary>
public class NotifySaveNameError : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject input_panel = default;           // Reference to Input Panel

    [SerializeField]
    GameObject keyboard = default;              // Reference to Keyboard Container

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Closes the Input/Keyboard window and
    /// opens a modal window to show an error in save name process.
    /// </summary>
    /// <param name="message">Error message.</param>
    public void notifySaveNameError(string message)
    {
        input_panel.SetActive(false);
        keyboard.SetActive(false);
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Return to Input/Keyboard Menu.
    /// </summary>
    private void okFunction()
    {
        input_panel.SetActive(true);
        keyboard.SetActive(true);
    }
}
