using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

/// <summary>
/// Implements the method to be confirmed by a modal window when a saved file game button is clicked
/// </summary>
public class ConfirmLoadGame : MonoBehaviour
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

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call
    /// </summary>
    public void confirmGame()
    {
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.Confirm("Would you like to load the game: " + Path.GetFileNameWithoutExtension(PlayerPrefs.GetString(GameFilesHandler.Saved_game)) + " ?", ok_action, cancel_action);
    }

    /// <summary>
    /// Load the Game Scene
    /// </summary>
    private void okFunction()
    {
        Debug.Log("Ok - " + PlayerPrefs.GetString(GameFilesHandler.Saved_game));
        // Load the GameScene (index 1)
        gameObject.GetComponent<LoadSceneOnClick>().LoadByIndex(1);
    }

    /// <summary>
    /// Return to the New Game Panel
    /// </summary>
    private void cancelFunction()
    {
        Debug.Log("Cancel - " + PlayerPrefs.GetString(GameFilesHandler.Saved_game));
        // Return to the parent panel
        parent_panel.SetActive(true);
    }
}
