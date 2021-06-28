using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the save choice when the return button is pressed in Display Menu.
/// </summary>
public class ConfirmSaveDisplay : MonoBehaviour
{
    private ModalPanel modal_panel;         // Modal panel refernce
    private GameFilesHandler files_handler; // Game Files Handler reference.

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
    /// Calls a modal panel to choose between save the current display settings configuration or not save.
    /// Cancel option means that the modifications will be lost only at the next execution.
    /// </summary>
    public void confirmSaveDisplay()
    {
        string question = "Would you like to save the Display Settings current configuration?";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Saves the current settings configuration and change the menu panel.
    /// </summary>
    private void okFunction()
    {
        files_handler.saveSettings(GameFilesHandler.Display_file);
        menu_panel.SetActive(true);
    }

    /// <summary>
    /// Only change the menu panel.
    /// </summary>
    private void cancelFunction()
    {
        menu_panel.SetActive(true);
    }

}
