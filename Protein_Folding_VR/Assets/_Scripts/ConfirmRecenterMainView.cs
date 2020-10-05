using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the modal panel calling to confirm the Main Menu recenter operation.
/// </summary>
public class ConfirmRecenterMainView : MonoBehaviour
{
    private ModalPanel modal_panel;             // Modal panel refernce

    [SerializeField]
    private GameObject player = default;        // Reference to player object

    [SerializeField]
    private GameObject menu_panel = default;    // Reference to return panel 

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Calls the modal panel and sets a string question.
    /// </summary>
    public void confirmRecenterMainView()
    {       
        string question = "To recenter your view, place your head in the new position and press the MENU joystick button. This operation can't be undone!";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Starts the Main Menu recenter coroutine.
    /// </summary>
    private void okFunction()
    {
        player.GetComponent<CoroutineRecenterMainView>().startRecenterMainView(menu_panel);
    }

    /// <summary>
    /// Return to previous menu panel.
    /// </summary>
    private void cancelFunction()
    {
        menu_panel.SetActive(true);
    }
}
