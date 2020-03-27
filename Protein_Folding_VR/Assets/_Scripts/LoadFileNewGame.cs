using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load the new game structure to be played
/// </summary>
public class LoadFileNewGame : MonoBehaviour
{
    /// <summary>
    /// Store the new game structure file name associated with the button (when clicked)
    /// into a Player Prefs field
    /// </summary>
    public void LoadName()
    {
        PlayerPrefs.SetString(GameFilesHandler.New_game, gameObject.GetComponent<GameListButton>().Button_file);
        Debug.Log("Loaded New Game File: " + PlayerPrefs.GetString(GameFilesHandler.New_game));
    }
}
