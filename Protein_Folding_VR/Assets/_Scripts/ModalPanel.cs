using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

// Implements genericaly the modal panel functions
public class ModalPanel : MonoBehaviour
{
    public Text question;
    public Button ok_button;
    public Button cancel_button;
    public GameObject modal_panel_obj;

    private static ModalPanel modal_panel;

    // Simple Singleton implementation
    public static ModalPanel Instance()
    {
        if (!modal_panel)
        {
            modal_panel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!modal_panel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }
        return modal_panel;
    }

    // Ok/Cancel: A string, a Ok event and a Cancel event
    public void Confirm(string question, UnityAction ok_event, UnityAction cancel_event)
    {
        modal_panel_obj.SetActive(true);

        // Remove all the listeners (like triggers to run functions) added before
        ok_button.onClick.RemoveAllListeners();
        // Add a listener - a reference to an action (function)
        ok_button.onClick.AddListener(ok_event);
        // Close the panel
        ok_button.onClick.AddListener(closePanel);

        cancel_button.onClick.RemoveAllListeners();
        cancel_button.onClick.AddListener(cancel_event);
        cancel_button.onClick.AddListener(closePanel);

        this.question.text = question;

        ok_button.gameObject.SetActive(true);
        cancel_button.gameObject.SetActive(true);
    }

    private void closePanel()
    {
        modal_panel_obj.SetActive(false);
    }
}
