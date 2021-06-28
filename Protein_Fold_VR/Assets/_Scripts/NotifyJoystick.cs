using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the absence joystick notification.
/// </summary>
public class NotifyJoystick : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject menu_panel = default;            // Reference to panel to return

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the joystick absence notifucation.
    /// </summary>
    public void notifyJoystickAbsence()
    {       
        string message = "Couldn't find a joystick. Please connect one and click OK to continue.";
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Activates the canvas and score objects.
    /// Calls the adjust display coroutine.
    /// </summary>
    private void okFunction()
    {
        menu_panel.SetActive(true);
    }
}
