using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that implements the tutorial text load from a file.
/// </summary>
public class LoadFileTutorial : MonoBehaviour
{
    private GameFilesHandler files_handler;     // Game Files Handler reference.

    [SerializeField]
    private Text tutorial_title = default;       // Reference to tutorial panel title viewer

    [SerializeField]
    private Text tutorial_text = default;       // Reference to tutorial panel text viewer

    private void Awake()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Read the text file to get the tutorial text.
    /// Print the text into the panel Text object.
    /// </summary>
    public void loadTutorialText()
    {
        // Read the text file to get the tutorial text
        // Print the text into the panel Text object
        tutorial_title.text = gameObject.GetComponent<GameListButton>().Button_text;
        tutorial_text.text = files_handler.readTxtFile(gameObject.GetComponent<GameListButton>().Button_file);       
    }
    
}
