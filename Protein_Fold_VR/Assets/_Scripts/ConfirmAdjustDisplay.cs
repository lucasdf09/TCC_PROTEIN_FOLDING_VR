using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the method to be confirmed by a modal window when an object display position is adjusted.
/// </summary>
public class ConfirmAdjustDisplay : MonoBehaviour
{
    private ModalPanel modal_panel;         // Modal panel refernce
    private Vector3 origin_position;        // Adjusting object origin position
    private GameFilesHandler files_handler; // Game Files Handler reference.

    [SerializeField]
    GameObject player_canvas = default;     // Object reference to adjusting object canvas

    [SerializeField]
    GameObject adjusting_object = default;  // Object reference to adjusting object

    [SerializeField]
    string adjusting_name = default;        // Object reference to adjusting object name in game

    [SerializeField]
    GameObject menu_panel = default;        // Reference to return panel

 
    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmPosition(Vector3 origin_position)
    {
        this.origin_position = origin_position;
        player_canvas.SetActive(false);
        string question = "Would you like to maintain the " + adjusting_name + " positon (Ok) or discart (Cancel)?";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Saves the new position.
    /// </summary>
    private void okFunction()
    {
        menu_panel.SetActive(true);
    }

    /// <summary>
    /// Reset to the position before the adjust.
    /// </summary>
    private void cancelFunction()
    {
        adjusting_object.GetComponent<RectTransform>().localPosition = origin_position;
        menu_panel.SetActive(true);
    }
}
