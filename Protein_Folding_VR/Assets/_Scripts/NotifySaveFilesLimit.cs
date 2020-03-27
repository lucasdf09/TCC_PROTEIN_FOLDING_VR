using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows in a modal window a message about the save files limit reached.
/// </summary>
public class NotifySaveFilesLimit : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject next_panel = default;        // Reference to the panel that called the notification

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Closes the menu panel that called the notification and
    /// opens a modal window to show an error in save files limit.
    /// </summary>
    /// <param name="message">Error message.</param>
    public void notifySaveFilesLimit(string message)
    {
        next_panel.SetActive(false);
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Return to the menu panel that called the notification.
    /// </summary>
    private void okFunction()
    {
        next_panel.SetActive(true);
    }
}
