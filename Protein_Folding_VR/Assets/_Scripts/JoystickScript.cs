using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickScript : MonoBehaviour
{
    private string joystickRead;

    public Text joystickText;

    // Start is called before the first frame update
    void Start()
    {
        joystickRead = "Starting!";
        setJoystickText(joystickRead);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            joystickRead = Input.inputString;
            Debug.Log(joystickRead);
            setJoystickText(joystickRead);

            if (Input.GetKey(KeyCode.Backspace))
                setJoystickText("Backspace");
            else if (Input.GetKey(KeyCode.Delete))
                setJoystickText("Delete");
            else if (Input.GetKey(KeyCode.Tab))
                setJoystickText("Tab");
            else if (Input.GetKey(KeyCode.Clear))
                setJoystickText("Clear");
            else if (Input.GetKey(KeyCode.Return))
                setJoystickText("Return");
            else if (Input.GetKey(KeyCode.Pause))
                setJoystickText("Pause");
            else if (Input.GetKey(KeyCode.Space))
                setJoystickText("Space");
            else if (Input.GetKey(KeyCode.UpArrow))
                setJoystickText("Up Arrow");
            else if (Input.GetKey(KeyCode.DownArrow))
                setJoystickText("Down Arrow");
            else if (Input.GetKey(KeyCode.RightArrow))
                setJoystickText("Right Arrow");
            else if (Input.GetKey(KeyCode.LeftArrow))
                setJoystickText("Left Arrow");
            else if (Input.GetKey(KeyCode.RightShift))
                setJoystickText("Right Shift");
            else if (Input.GetKey(KeyCode.LeftShift))
                setJoystickText("Left Shift");
            else if (Input.GetKey(KeyCode.RightControl))
                setJoystickText("Right Control");
            else if (Input.GetKey(KeyCode.LeftControl))
                setJoystickText("Left Control");
            else if (Input.GetKey(KeyCode.RightAlt))
                setJoystickText("Right Alt");
            else if (Input.GetKey(KeyCode.LeftAlt))
                setJoystickText("Left Alt");
            else if (Input.GetKey(KeyCode.JoystickButton0))
                setJoystickText("JoystickButton0");
            else if (Input.GetKey(KeyCode.JoystickButton1))
                setJoystickText("JoystickButton1");
            else if (Input.GetKey(KeyCode.Escape))
            {
                setJoystickText("Escape");
            }
                
        }
        else
        {
            if (Input.GetAxis("Horizontal") != 0)
                setJoystickText("H " + Input.GetAxis("Horizontal").ToString());

            if (Input.GetAxis("Vertical") != 0)
                setJoystickText("V " + Input.GetAxis("Vertical").ToString());

            if (Input.GetAxis("Z-axis") != 0)
                setJoystickText("Z " + Input.GetAxis("Z-axis").ToString());       
        }     
    }

    void setJoystickText (string printText)
    {
        joystickText.text = "Joystick: " + printText;
    }
}
