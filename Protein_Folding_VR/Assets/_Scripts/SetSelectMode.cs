using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Set select mode to true in Player Controler after leaving the game menu
public class SetSelectMode : MonoBehaviour
{
    public void activateSelectMode()
    {
        PlayerController.select_mode = true;
    }
}
