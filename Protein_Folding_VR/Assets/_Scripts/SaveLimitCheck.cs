using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tests the saved files limit number.
/// </summary>
public class SaveLimitCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject save_panel = default;

    [SerializeField]
    private GameObject input_panel = default;

    [SerializeField]
    private GameObject keyboard_container = default;

    /// <summary>
    /// Go to input/keyboard if the saved files limit isn't reached.
    /// </summary>
    public void testLimit()
    {
        if(GameFilesHandler.countSavesFolder() < GameFilesHandler.Saves_limit)
        {
            // Hide Save Menu
            save_panel.SetActive(false);
            // Show Input Panel
            input_panel.SetActive(true);
            // Set Input text message
            input_panel.GetComponent<InputPanelSetText>().setInputText("Entry the file name:");
            //Show Keyboard
            keyboard_container.SetActive(true);
        }
        else
        {
            // Show Error message
            Debug.Log("Save files limit reached!");
            gameObject.GetComponent<NotifySaveFilesLimit>().notifySaveFilesLimit("Can't Save:\nSave files limit reached!\nTry removing some save file.");
        }
    }
}
