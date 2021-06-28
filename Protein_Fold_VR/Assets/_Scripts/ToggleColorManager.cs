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

    private void Awake()
    {
        // Set the button states colors
        normal_color = new Color(0.0f, 0.0f, 0.0f);             // Black
        highlight_color = new Color(0.0f, 0.0f, 1.0f, 0.5f);    // Semitransparent Blue
        pressed_color = new Color(0.0f, 0.0f, 1.0f);            // Blue
    }

    private void Start()
    {
        background.color = normal_color;
        checkmark.color = normal_color;       
        label.color = normal_color;
    }

    /// <summary>
    /// Set the button's color to Semitransparent Blue.
    /// </summary>
    public void highlightedButton()
    {
        background.color = highlight_color;
        checkmark.color = highlight_color;        
        label.color = highlight_color;
    }

    /// <summary>
    /// Set the button's color to Black.
    /// </summary>
    public void normalizedButton()
    {
        background.color = normal_color;
        checkmark.color = normal_color;        
        label.color = normal_color;
    }

    /// <summary>
    /// Set the button's color to Blue.
    /// </summary>
    public void pressedButton()
    {
        background.color = pressed_color;
        checkmark.color = pressed_color;        
        label.color = pressed_color;
    }
}
