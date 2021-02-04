using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modal window invallid new game (input file) notification implementation.
/// </summary>
public class NotifyNewGameInvalid : MonoBehaviour
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
    /// Opens a modal window to show the new game file status.
    /// </summary>
    /// <param name="message">Status message.</param>
    public void notifyInvalid(string message)
    {
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Return to New Game Panel.
    /// </summary>
    private void okFunction()
    {
        next_panel.SetActive(true);
    }
}
