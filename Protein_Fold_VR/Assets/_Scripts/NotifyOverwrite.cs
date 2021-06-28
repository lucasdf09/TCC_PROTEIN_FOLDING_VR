using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modal window overwrite notification implementation.
/// </summary>
public class NotifyOverwrite : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject next_panel = default;            // Reference to panel to show after closing

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the overwrite file status.
    /// </summary>
    /// <param name="message">Status message.</param>
    public void notifyOverwrite(string message)
    {
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
