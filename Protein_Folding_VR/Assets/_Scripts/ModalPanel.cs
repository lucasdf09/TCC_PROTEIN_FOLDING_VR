using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

/// <summary>
/// Implements genericaly the Modal Panel functions.
/// </summary>
public class ModalPanel : MonoBehaviour
{
    public Text panel_text;
    public Button ok_button;
    public Button cancel_button;
    public GameObject modal_panel_obj;

    private static ModalPanel modal_panel;

    /// <summary>
    /// Simple Singleton implementation.
    /// </summary>
    /// <returns>Modal panel reference.</returns>
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

    /// <summary>
    /// Set the confirm panel question text and ok/cancel buttons operations.
    /// </summary>
    /// <param name="question">Question text.</param>
    /// <param name="ok_event">Ok action reference.</param>
    /// <param name="cancel_event">Cancel action reference.</param>
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

        this.panel_text.text = question;

        ok_button.gameObject.SetActive(true);
        cancel_button.gameObject.SetActive(true);
    }

    /// <summary>
    /// Set the confirm panel question text and ok/cancel buttons operations before a notify panel.
    /// </summary>
    /// <param name="question">Question text.</param>
    /// <param name="ok_event">Ok action reference.</param>
    /// <param name="cancel_event">Cancel action reference.</param>
    public void ConfirmAndNotify(string question, UnityAction ok_event, UnityAction cancel_event)
    {
        modal_panel_obj.SetActive(true);

        // Remove all the listeners (like triggers to run functions) added before
        ok_button.onClick.RemoveAllListeners();
        // Add a listener - a reference to an action (function)
        ok_button.onClick.AddListener(ok_event);
        // Reopen the panel after a time delay
        ok_button.onClick.AddListener(resetPanel);

        cancel_button.onClick.RemoveAllListeners();
        cancel_button.onClick.AddListener(cancel_event);
        cancel_button.onClick.AddListener(closePanel);

        this.panel_text.text = question;

        ok_button.gameObject.SetActive(true);
        cancel_button.gameObject.SetActive(true);
    }

    /// <summary>
    /// Set the notify window message and ok button operation. 
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="ok_event">Ok action reference.</param>
    public void Notify(string message, UnityAction ok_event)
    {
        modal_panel_obj.SetActive(true);

        // Remove all the listeners (like triggers to run functions) added before
        ok_button.onClick.RemoveAllListeners();
        // Add a listener - a reference to an action (function)
        ok_button.onClick.AddListener(ok_event);
        // Close the panel
        ok_button.onClick.AddListener(closePanel);

        this.panel_text.text = message;

        ok_button.gameObject.SetActive(true);
        cancel_button.gameObject.SetActive(false);
    }

    /// <summary>
    /// Close the modal panel.
    /// </summary>
    private void closePanel()
    {
        modal_panel_obj.SetActive(false);
    }

    /// <summary>
    /// Open the modal panel.
    /// </summary>
    private void resetPanel()
    {
        StartCoroutine(waiter());
    }

    /// <summary>
    /// Waits for a time between the on/off of the modal panel.
    /// </summary>
    /// <returns></returns>
    private IEnumerator waiter()
    {
        modal_panel_obj.SetActive(false);

        //Wait for 1 seconds
        yield return new WaitForSeconds(0.1f);

        modal_panel_obj.SetActive(true);
    }
}
