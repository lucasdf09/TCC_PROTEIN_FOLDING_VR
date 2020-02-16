using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

// Implements the functions to be confirmed by a modal window when specific types of buttons are clicked
public class ConfirmNewGame : MonoBehaviour
{
    private ModalPanel modal_panel;

    private UnityAction ok_action;
    private UnityAction cancel_action;

    [SerializeField]
    GameObject parent_panel;

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        ok_action = new UnityAction(okFunction);
        cancel_action = new UnityAction(cancelFunction);
    }

    // Send to the Modal Panel to set up the Buttons and Functions to call
    public void confirmNewGame()
    {
        modal_panel.Confirm("Would you like to start a new game with " + Path.GetFileNameWithoutExtension(PlayerPrefs.GetString("File_Name")) + " ?", ok_action, cancel_action);
    }

    // These are wrapped into UnityActions
    private void okFunction()
    {
        Debug.Log("Ok - " + PlayerPrefs.GetString("File_Name"));
        // Load the GameScene (index 1)
        gameObject.GetComponent<LoadSceneOnClick>().LoadByIndex(1);
    }

    private void cancelFunction()
    {
        Debug.Log("Cancel - " + PlayerPrefs.GetString("File_Name"));
        // Return to the parent panel
        parent_panel.SetActive(true);
    }
}
