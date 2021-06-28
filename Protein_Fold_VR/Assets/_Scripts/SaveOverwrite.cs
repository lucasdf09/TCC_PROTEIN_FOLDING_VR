using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Overwrite methods.
/// </summary>
public class SaveOverwrite : MonoBehaviour
{
    private GameFilesHandler files_handler;     //Reference to Game Files Handler

    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Overwrite the Saved game file.
    /// </summary>
    public void saveOverwrite()
    {   
        // Verify the existance of a Saved file
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.Saved_game)))
        {
            gameObject.GetComponent<ConfirmOverwrite>().confirmOverwrite();
        }
        else
        {
            Debug.Log("Can't overwrite: Saved game not found!");
            // Notify the missing of a saved game
            // Always occurs in a new game, before a new save
            gameObject.GetComponent<NotifyOverwrite>().notifyOverwrite("Can't overwrite:\nSaved game not found!\nTry a new save.");
        }
    }
}
