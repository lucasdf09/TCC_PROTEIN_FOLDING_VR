using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListButton : MonoBehaviour
{
    [SerializeField]
    private Text button_text;

    [SerializeField]
    private GameListController list_controller;

    private string button_file;

    private string msg_text;

    // Properties
    public string Button_file
    {
        get
        {
            return button_file;
        }
        set
        {
            button_file = value;
        }
    }
    
    public string Button_text
    {
        set
        {
            button_text.text = value;
            msg_text = value;
        }
    }

    /*
    public void setText(string text_string)
    {
        msg_text = text_string;
        button_text.text = text_string;
    }
    

    public void OnClick()
    {
        list_controller.buttonClicked(msg_text);
    }
    */
}