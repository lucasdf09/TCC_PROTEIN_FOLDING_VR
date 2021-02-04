using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load the saved game structure to be played.
/// </summary>
public class LoadFileLoadGame : MonoBehaviour
{
    /// <summary>
    /// Store the saved game structure file name associated with the button (when clicked) into a Player Prefs field.
    /// </summary>
    public void LoadName()
    {
        PlayerPrefs.SetString(GameFilesHandler.Saved_game, gameObject.GetComponent<GameListButton>().Button_file);
        Debug.Log("Loaded Input File: " + PlayerPrefs.GetString(GameFilesHandler.Saved_game));
    }
}
