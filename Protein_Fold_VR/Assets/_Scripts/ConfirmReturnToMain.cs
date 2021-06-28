using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Class that implements the modal panel calling to confirm the return to Main Menu.
/// </summary>
public class ConfirmReturnToMain : MonoBehaviour
{
    private ModalPanel modal_panel;

    //private UnityAction ok_action;
    //private UnityAction cancel_action;

    [SerializeField]
    private GameObject parent_panel = default;

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        //ok_action = new UnityAction(okFunction);
        //cancel_action = new UnityAction(cancelFunction);
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmReturnToMain()
    {
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.Confirm("All the unsaved progress will be lost. Do you want to go to Main Menu ?", okFunction, cancelFunction);
    }

    /// <summary>
    /// Loads the Main Menu Scene.
    /// </summary>
    private void okFunction()
    {
        gameObject.GetComponent<LoadSceneOnClick>().LoadByIndex(0);
    }

    /// <summary>
    /// Return to Game Menu.
    /// </summary>
    private void cancelFunction()
    {
        // Return to the parent panel
        parent_panel.SetActive(true);
    }
}
