using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using System.IO;

public class ConfirmExit : MonoBehaviour
{
    private ModalPanel modal_panel;

    //private UnityAction ok_action;
    //private UnityAction cancel_action;

    [SerializeField]
    private GameObject parent_panel;

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        //ok_action = new UnityAction(okFunction);
        //cancel_action = new UnityAction(cancelFunction);
    }

    // Send to the Modal Panel to set up the Buttons and Functions to call
    public void confirmExit()
    {
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.Confirm("All the unsaved progress will be lost. Do you want to exit ?", okFunction, cancelFunction);
    }

    // These are wrapped into UnityActions
    private void okFunction()
    {
        // Exit the game
        gameObject.GetComponent<ExitOnClick>().Exit();
    }

    private void cancelFunction()
    {        
        // Return to the parent panel
        parent_panel.SetActive(true);
    }
}
