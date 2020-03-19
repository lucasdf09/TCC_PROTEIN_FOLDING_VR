using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens a modal window to confirm the remotion of a saved file.
/// </summary>
public class ConfirmRemoveSave : MonoBehaviour
{
    private ModalPanel modal_panel;             // Modal panel refernce

    private GameFilesHandler files_handler;     // Game Files Handler reference

    [SerializeField]
    GameObject parent_panel = default;          // Panel to return reference

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Sets the Modal Panel behaviour to remove a saved file.
    /// </summary>
    public void confirmRemoveSave()
    {
        string file_name = gameObject.GetComponent<GameListButton>().Button_text;
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.ConfirmAndNotify("Would you like to remove:\n" + file_name + "?", okFunction, cancelFunction);
    }

    /// <summary>
    /// Remove the file and the button object.
    /// </summary>
    private void okFunction()
    {
        // Clear the Player Prefs Saved file reference
        // Button associated file_path string
        string file_path = gameObject.GetComponent<GameListButton>().Button_file;
        // Player Prefs saved file associated string
        string player_path = PlayerPrefs.GetString(GameFilesHandler.Saved_game);
        // If the player want to remove the actual game, remove the reference too
        if (string.Equals(file_path, player_path))
        {
            PlayerPrefs.SetString(GameFilesHandler.Saved_game, null);
        }

        // Remove the save file
        string file_name = gameObject.GetComponent<GameListButton>().Button_text;
        files_handler.removeSaveFile(file_name);

        // Check the remotion and notify the player
        if (!files_handler.saveFileExists(file_name))
        {
            gameObject.GetComponent<NotifyRemove>().notifyRemove(file_name + "\nSuccessfully removed!");
        }
        else
        {
            gameObject.GetComponent<NotifyRemove>().notifyRemove(file_name + "\nCouldn't be removed!");
        }
    }

    /// <summary>
    /// Return to Game Remove Panel.
    /// </summary>
    private void cancelFunction()
    {
        // Activates the parent panel
        parent_panel.SetActive(true);
    }
}
