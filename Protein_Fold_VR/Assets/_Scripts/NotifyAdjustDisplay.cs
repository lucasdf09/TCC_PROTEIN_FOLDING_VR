using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Modal window adjust score notify implementation.
/// </summary>
public class NotifyAdjustDisplay : MonoBehaviour
{
    private ModalPanel modal_panel;             // Reference to Modal Panel

    [SerializeField]
    GameObject player_canvas = default;         // Object reference to adjusting object canvas

    [SerializeField]
    GameObject adjusting_object = default;      // Object reference to adjusting object

    [SerializeField]
    string adjusting_name = default;            // Object reference to adjusting object name in game

    [SerializeField]
    GameObject toggle_button = default;         // Adjusting object toogle button reference

    [SerializeField]
    GameObject menu_panel = default;            // Reference to return panel

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the adjust score instructions.
    /// </summary>
    public void notifyAdjustPosition()
    {
        if (toggle_button.GetComponent<Toggle>().isOn)
        {
            string message = "Adjust the position of the " + adjusting_name + " using the movement commands on the X, Y and Z axes. Press the MENU joystick button to finish.";
            modal_panel.Notify(message, okFunction);
        }
        else
        {
            string message = "Can't adjust " + adjusting_name + ".\nThe object display needs to be visible to be adjusted.";
            modal_panel.Notify(message, noOkFunction);
        }       
    }

    /// <summary>
    /// Activates the canvas and score objects.
    /// Calls the adjust display coroutine.
    /// </summary>
    private void okFunction()
    {
        player_canvas.SetActive(true);       
        player_canvas.GetComponent<CoroutineAdjustDisplay>().StartAdjustPosition(adjusting_object, gameObject);
    }

    /// <summary>
    /// Shows a warning message and returns to the Game Display Panel.
    /// </summary>
    private void noOkFunction()
    {
        menu_panel.SetActive(true);
    }
}
