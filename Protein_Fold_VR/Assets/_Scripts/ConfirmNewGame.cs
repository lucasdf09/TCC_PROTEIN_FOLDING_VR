using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

/// <summary>
/// Implements the method to be confirmed by a modal window when a new game button is clicked.
/// </summary>
public class ConfirmNewGame : MonoBehaviour
{
    private ModalPanel modal_panel;     // Modal panel refernce

    private UnityAction ok_action;      // Ok method reference
    private UnityAction cancel_action;  // Cancel method reference

    [SerializeField]
    GameObject parent_panel = default;  // Panel to return reference

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        ok_action = new UnityAction(okFunction);
        cancel_action = new UnityAction(cancelFunction);
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmGame()
    {
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.Confirm("Would you like to start a game with:\n" + Path.GetFileNameWithoutExtension(PlayerPrefs.GetString(GameFilesHandler.New_game)) + "?", ok_action, cancel_action);
    }

    /// <summary>
    /// Load the Game Scene.
    /// </summary>
    private void okFunction()
    {
        Debug.Log("Ok - " + PlayerPrefs.GetString(GameFilesHandler.New_game));
        // Load the GameScene (index 1)
        gameObject.GetComponent<LoadSceneOnClick>().LoadByIndex(1);
    }

    /// <summary>
    /// Return to the New Game Panel.
    /// </summary>
    private void cancelFunction()
    {
        Debug.Log("Cancel - " + PlayerPrefs.GetString(GameFilesHandler.New_game));
        // Clear the PlayerPrefs field
        PlayerPrefs.SetString(GameFilesHandler.New_game, null);
        // Activates the parent panel
        parent_panel.SetActive(true);
    }
}
