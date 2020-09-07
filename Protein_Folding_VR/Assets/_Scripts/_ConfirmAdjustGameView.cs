using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the method to be confirmed by a modal window when the player game view is adjusted.
/// </summary>
public class ConfirmAdjustGameView : MonoBehaviour
{
    private ModalPanel modal_panel;         // Modal panel refernce
    private Quaternion previous_rotation;   // Player rotation before the adjust
    private Vector3 light_position;         // Directinal light position before adjust

    [SerializeField]
    private GameObject player = default;        // Reference to player object

    [SerializeField]
    private GameObject structure = default;     // Reference to the structure

    [SerializeField]
    private GameObject menu_panel = default;    // Reference to return panel

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmAdjustGameView(Quaternion previous_rotation, Vector3 light_position)
    {
        this.previous_rotation = previous_rotation;
        this.light_position = light_position;
        structure.SetActive(false);
        string question = "Would you like to maintain the adjusted game view (Ok) or discart (Cancel)?";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Manintain the new player rotation.
    /// </summary>
    private void okFunction()
    {
        menu_panel.SetActive(true);
    }

    /// <summary>
    /// Reset to the rotation before the adjust.
    /// </summary>
    private void cancelFunction()
    {
        player.transform.rotation = previous_rotation;
        menu_panel.SetActive(true);
    }
}
