using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the color effects in the toggle buttons during the user actions.
/// </summary>
public class ToggleColorManager : MonoBehaviour
{
    public Image background;
    public Image checkmark;    
    public Text label;

    public static Color normal_color;
    public static Color highlight_color;
    public static Color pressed_color;
    //public static Color normal_text;
    //public static Color highlight_text;
    //public static Color pressed_text;

    private void Awake()
    {
        // Set the button states colors
        normal_color = new Color(0.0f, 0.0f, 0.0f);             // Black
        highlight_color = new Color(0.0f, 0.0f, 1.0f, 0.5f);    // Semitransparent Blue
        pressed_color = new Color(0.0f, 0.0f, 1.0f);            // Blue

        // Set the button text states colors
        //normal_text = new Color(0.0f, 0.0f, 0.0f);              // Black
        //highlight_text = new Color(0.0f, 0.0f, 1.0f, 0.5f);     // Semitransparent Blue
        //pressed_text = new Color(0.0f, 0.0f, 1.0f);             // Blue
    }

    private void Start()
    {
        // Set the color buttons to Black
        //gameObject.GetComponent<Image>().color = normal_color;
        //gameObject.GetComponentInChildren<Text>().color = normal_color;
        background.color = normal_color;
        checkmark.color = normal_color;       
        label.color = normal_color;
    }

    /// <summary>
    /// Set the button's color to Semitransparent Blue
    /// </summary>
    public void highlightedButton()
    {
        //gameObject.GetComponent<Image>().color = highlight_color;
        //gameObject.GetComponentInChildren<Text>().color = highlight_color;
        background.color = highlight_color;
        checkmark.color = highlight_color;        
        label.color = highlight_color;
    }

    /// <summary>
    /// Set the button's color to Black
    /// </summary>
    public void normalizedButton()
    {
        //gameObject.GetComponent<Image>().color = normal_color;
        //gameObject.GetComponentInChildren<Text>().color = normal_color;
        background.color = normal_color;
        checkmark.color = normal_color;        
        label.color = normal_color;
    }

    /// <summary>
    /// Set the button's color to Blue
    /// </summary>
    public void pressedButton()
    {
        //gameObject.GetComponent<Image>().color = pressed_color;
        //gameObject.GetComponentInChildren<Text>().color = pressed_color;
        background.color = pressed_color;
        checkmark.color = pressed_color;        
        label.color = pressed_color;
    }
}
