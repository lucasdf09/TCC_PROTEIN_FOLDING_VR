using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Ask for a confirmation to overwrites the saved file loaded.
/// </summary>
public class ConfirmOverwrite : MonoBehaviour
{
    private ModalPanel modal_panel;             // Modal panel refernce

    private GameFilesHandler files_handler;     //Reference to Game Files Handler

    [SerializeField]
    GameObject previous_panel = default;        // Panel to return reference


    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Set the modal window behaviour for an overwrite oeration.
    /// </summary>
    public void confirmOverwrite()
    {
        // Saved file path
        string file_name = PlayerPrefs.GetString(GameFilesHandler.Saved_game);
        // Exrtact only the saved file name
        file_name = Path.GetFileNameWithoutExtension(file_name);
        // Calls the Confirm function with: message to be shown, Ok assigned function, Cancel assigned function
        modal_panel.ConfirmAndNotify("Would you like to overwrite:\n" + file_name + "?", okFunction, cancelFunction);
    }

    /// <summary>
    /// Overwrite the Saved game.
    /// </summary>
    private void okFunction()
    {
        // Saved file path
        string file_name = PlayerPrefs.GetString(GameFilesHandler.Saved_game);
        // Exrtact only the saved file name
        file_name = Path.GetFileNameWithoutExtension(file_name);
        files_handler.saveGame(file_name);

        // Overwrite the output file associated
        files_handler.saveOutput(file_name);

        // Verify if the file was successfully overwrited
        if (files_handler.saveFileExists(file_name))
        {
            Debug.Log("Overwrite successful!");
            gameObject.GetComponent<NotifyOverwrite>().notifyOverwrite(file_name + "\nOverwrite successful!");
        }
        else
        {
            gameObject.GetComponent<NotifyOverwrite>().notifyOverwrite(file_name + "\nCouldn't be overwrited!");
        }

        // Verify if the output file was successfully overwrited
        if (files_handler.outputFileExists(file_name))
        {          
            Debug.Log(file_name + "\nOutput overwrite successful!");
        }
        else
        {
            Debug.Log(file_name + "\nCouldn't be overwrited in Outputs!");
        }
    }

    /// <summary>
    /// Return to the Save Game Panel.
    /// </summary>
    private void cancelFunction()
    {
        // Activates the parent panel
        previous_panel.SetActive(true);
    }
}
