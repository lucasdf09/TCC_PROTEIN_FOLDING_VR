using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to implements the Return operation in the Tutorial Info Panel.
/// </summary>
public class TutorialsReturnButton : MonoBehaviour
{
    GameObject return_panel;        // Reference to the panel to return

    /// <summary>
    /// Sets the return panel game object.
    /// </summary>
    /// <param name="return_panel"></param>
    public void setReturnPanel(GameObject return_panel)
    {
        this.return_panel = return_panel;
    }

    /// <summary>
    /// Activates the return panel game object (make it visible).
    /// </summary>
    public void activateReturnPanel()
    {
        return_panel.SetActive(true);
    }
}
