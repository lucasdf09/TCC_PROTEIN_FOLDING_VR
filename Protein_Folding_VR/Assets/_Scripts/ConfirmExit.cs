using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exits the game asking for a confirmation.
/// </summary>
public class ConfirmExit : MonoBehaviour
{
    private ModalPanel modal_panel;                 // Reference to Modal Panel

    [SerializeField]
    private GameObject previous_panel = default;    // Reference to the panel that called the Modal Panel

    private void Awake()
    {
        // Modal Panel reference initialization (singleton)
        modal_panel = ModalPanel.Instance();        
    }

    /// <summary>
    /// Send to the Modal Panel, to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmExit()
    {
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.Confirm("All the unsaved progress will be lost. Do you want to exit ?", okFunction, cancelFunction);
    }

    /// <summary>
    /// Set the Ok modal button to exit the application.
    /// </summary>
    private void okFunction()
    {
        // Exit the game
        gameObject.GetComponent<ExitOnClick>().Exit();
    }

    /// <summary>
    /// Set the Cancel modal button to return to the previous panel.
    /// </summary>
    private void cancelFunction()
    {        
        // Return to the previous panel
        previous_panel.SetActive(true);
    }
}
