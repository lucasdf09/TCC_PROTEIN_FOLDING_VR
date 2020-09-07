using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the player menu view reset.
/// </summary>
public class ConfirmResetMenuView : MonoBehaviour
{
    private ModalPanel modal_panel;     // Modal panel refernce

    [SerializeField]
    GameObject player = default;        // Reference to player object

    [SerializeField]
    GameObject menu_panel = default;    // Reference to return panel
    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmResetMenuView()
    {
        string question = "Would you like to reset the player menu view?";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Reset the player rotation.
    /// </summary>
    private void okFunction()
    {
        //player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        player.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        menu_panel.SetActive(true);
    }

    /// <summary>
    /// Return to Menu Options Panel.
    /// </summary>
    private void cancelFunction()
    {
        menu_panel.SetActive(true);
    }
}
