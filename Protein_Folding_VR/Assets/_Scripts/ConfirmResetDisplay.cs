using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the screen displays position. 
/// </summary>
public class ConfirmResetDisplay : MonoBehaviour
{
    private ModalPanel modal_panel;     // Modal panel refernce

    [SerializeField]
    GameObject score = default;         // Object reference to Score

    [SerializeField]
    GameObject score_toggle = default;         // Object reference to Score Toggle

    [SerializeField]
    GameObject parameters = default;    // Object reference to Parameters

    [SerializeField]
    GameObject parameters_toggle = default;    // Object reference to Parameters Toggle

    [SerializeField]
    GameObject menu_panel = default;    // Reference to return panel

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// Send to the Modal Panel to set up the Buttons and Functions to call.
    /// </summary>
    public void confirmResetDisplay()
    {
        string question = "Would you like to reset all the Display settings?";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// Reset the Score and Parameters settings (including toggles).
    /// </summary>
    private void okFunction()
    {
        score.GetComponent<RectTransform>().localPosition = new Vector3(0, 1.5f, 3);
        score_toggle.GetComponent<Toggle>().isOn = true;      
        parameters.GetComponent<RectTransform>().localPosition = new Vector3(-1.2f, 0.3f, 3);
        parameters_toggle.GetComponent<Toggle>().isOn = false;
        menu_panel.SetActive(true);
    }

    /// <summary>
    /// Return to Game Display Panel.
    /// </summary>
    private void cancelFunction()
    {
        menu_panel.SetActive(true);
    }


}
