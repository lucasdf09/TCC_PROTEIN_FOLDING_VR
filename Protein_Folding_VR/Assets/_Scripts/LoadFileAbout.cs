using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that implements the about application text load from a file.
/// </summary>
public class LoadFileAbout : MonoBehaviour
{
    private GameFilesHandler files_handler;     // Game Files Handler reference.

    [SerializeField]
    private Text panel_title = default;         // Reference to tutorial panel title viewer

    [SerializeField]
    private Text panel_text = default;          // Reference to tutorial panel text viewer

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
    public void loadAboutText()
    {
        // Read the text file to get the tutorial text
        // Print the text into the panel Text object
        panel_title.text = "About Application";

        string about_file_text = files_handler.getAboutText();

        if (!string.IsNullOrWhiteSpace(about_file_text))
        {
            panel_text.text = about_file_text;
        }
        else
        {
            panel_text.text = "About file couldn't be loaded!";
        }
    }
}
