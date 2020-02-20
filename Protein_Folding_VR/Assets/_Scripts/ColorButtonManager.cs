using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the color effects during the user actions in the buttons
public class ColorButtonManager : MonoBehaviour
{
    public static Color normal_color;
    public static Color highlight_color;
    public static Color pressed_color;
    public static Color normal_text;
    public static Color highlight_text;
    public static Color pressed_text;

    private void Awake()
    {
        normal_color = new Color(0.0f, 0.0f, 0.0f);
        highlight_color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        pressed_color = new Color(0.0f, 0.0f, 1.0f);
        normal_text = new Color(0.0f, 0.0f, 0.0f);
        highlight_text = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        pressed_text = new Color(0.0f, 0.0f, 1.0f);
    }

    private void Start()
    {
        gameObject.GetComponent<Image>().color = normal_color;
        gameObject.GetComponentInChildren<Text>().color = normal_text;
    }

    public void highlightedButton()
    {
        gameObject.GetComponent<Image>().color = highlight_color;
        gameObject.GetComponentInChildren<Text>().color = highlight_text;

    }

    public void normalizedButton()
    {
        gameObject.GetComponent<Image>().color = normal_color;
        gameObject.GetComponentInChildren<Text>().color = normal_text;
    }

    public void pressedButton()
    {
        gameObject.GetComponent<Image>().color = pressed_color;
        gameObject.GetComponentInChildren<Text>().color = pressed_text;
    }
}
