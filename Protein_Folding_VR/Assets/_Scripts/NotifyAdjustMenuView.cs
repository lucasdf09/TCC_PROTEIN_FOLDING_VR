using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modal window adjust player menu view notify implementation.
/// </summary>
public class NotifyAdjustMenuView : MonoBehaviour
{
    private ModalPanel modal_panel;         // Reference to Modal Panel

    [SerializeField]
    GameObject player = default;            // Reference to player object

    [SerializeField]
    GameObject menu_panel = default;        // Reference to return panel

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the adjust menu view instructions.
    /// Used too as a reference.
    /// </summary>
    public void notifyAdjustMenuView()
    {
        string message = "Adjust the rotation of your view using the movement commands on the X, Y and Z axes. Press OK to finish.";
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Calls the adjust menu view coroutine.
    /// </summary>
    private void okFunction()
    {
        player.GetComponent<CoroutineAdjustMenuView>().StopAdjustMenuView();
        menu_panel.SetActive(true);
    }
}
