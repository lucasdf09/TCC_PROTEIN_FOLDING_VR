using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementes the method to set select mode to true in Player Controler after leaving the game menu.
/// </summary>
public class SetSelectMode : MonoBehaviour
{
    /// <summary>
    /// Set select mode to true in Player Controler after leaving the game menu.
    /// </summary>
    public void activateSelectMode()
    {
        PlayerController.select_mode = true;
    }
}
