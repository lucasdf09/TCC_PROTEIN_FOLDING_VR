using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Buttons attributes and parent component references
/// </summary>
public class GameListButton : MonoBehaviour
{
    [SerializeField]
    private Text button_text = default;                     // Button text

    [SerializeField]
    private GameListController list_controller = default;   // Reference to parent Game List Controller script

    private string button_file;                             // Game file name associated with the button

    private string msg_text;

    /// <summary>
    /// Get and Set methods to the file name associated
    /// </summary>
    public string Button_file
    {       
        get
        {
            return button_file;
        }
        set
        {
            button_file = value;
        }
    }

    /// <summary>
    /// Set the button text to be shown on game
    /// </summary>
    public string Button_text
    {
        get
        {
            return button_text.text;
        }
        set
        {
            button_text.text = value;
            msg_text = value;
        }
    }
}