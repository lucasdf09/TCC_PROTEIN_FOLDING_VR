using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ConfirmRecenterMainView : MonoBehaviour
{
    private ModalPanel modal_panel;             // Modal panel refernce

    [SerializeField]
    private GameObject player = default;        // Reference to player object

    [SerializeField]
    private GameObject menu_panel = default;    // Reference to return panel 

    private void Awake()
    {
        modal_panel = ModalPanel.Instance();
    }

    /// <summary>
    /// 
    /// </summary>
    public void confirmRecenterMainView()
    {       
        string question = "To recenter your view, place your head in the new position and press the MENU joystick button.";
        modal_panel.Confirm(question, okFunction, cancelFunction);
    }

    /// <summary>
    /// 
    /// </summary>
    private void okFunction()
    {
        player.GetComponent<CoroutineRecenterMainView>().startRecenterMainView(menu_panel);
    }

    /// <summary>
    /// 
    /// </summary>
    private void cancelFunction()
    {
        menu_panel.SetActive(true);
    }
}
