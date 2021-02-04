using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modal window implementation to file remotion notification.
/// </summary>
public class NotifyRemove : MonoBehaviour
{
    private ModalPanel modal_panel;                 // Reference to Modal Panel

    [SerializeField]
    GameObject next_panel = default;                // Reference to panel to show after closing

    [SerializeField]
    GameListController list_controller = default;   // Reference to Game List

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the removed file status.
    /// </summary>
    /// <param name="message">Status message.</param>
    public void notifyRemove(string message)
    {
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Return to Remove Menu.
    /// </summary>
    private void okFunction()
    {
        next_panel.SetActive(true);
        list_controller.generateList();
    }
}
