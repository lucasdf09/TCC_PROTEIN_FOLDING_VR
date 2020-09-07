using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to implements the adjust player view modal window call.
/// </summary>
public class NotifyAdjustGameView : MonoBehaviour
{
    private ModalPanel modal_panel;         // Reference to Modal Panel

    [SerializeField]
    GameObject player = default;            // Reference to player object

    [SerializeField]
    GameObject structure = default;         // Reference to structure object

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Opens a modal window to show the adjust game view instructions.
    /// </summary>
    public void notifyAdjustGameView()
    {
        string message = "Adjust the rotation of your view using the movement commands on the X, Y and Z axes. Press the MENU joystick button to finish.";
        modal_panel.Notify(message, okFunction);
    }

    /// <summary>
    /// Calls the adjust game view coroutine.
    /// </summary>
    private void okFunction()
    {
        player.GetComponent<CoroutineAdjustGameView>().StartAdjustGameView(structure, gameObject);
        structure.SetActive(true);
    }
}
