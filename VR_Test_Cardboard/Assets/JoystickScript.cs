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
            else if (Input.GetKey(KeyCode.Escape))
                setJoystickText("Escape");
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
            else if (Input.GetKey(KeyCode.Insert))
                setJoystickText("Insert");
            else if (Input.GetKey(KeyCode.Home))
                setJoystickText("Home");
            else if (Input.GetKey(KeyCode.End))
                setJoystickText("End");
            else if (Input.GetKey(KeyCode.At))
                setJoystickText("@");
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
            else if (Input.GetKey(KeyCode.AltGr))
                setJoystickText("AltGr");
            else if (Input.GetKey(KeyCode.JoystickButton0))
                setJoystickText("JoystickButton0");
            else if (Input.GetKey(KeyCode.JoystickButton1))
                setJoystickText("JoystickButton1");
            else if (Input.GetKey(KeyCode.JoystickButton2))
                setJoystickText("JoystickButton2");
            else if (Input.GetKey(KeyCode.JoystickButton3))
                setJoystickText("JoystickButton3");
            else if (Input.GetKey(KeyCode.JoystickButton4))
                setJoystickText("JoystickButton4");
            else if (Input.GetKey(KeyCode.JoystickButton5))
                setJoystickText("JoystickButton5");
            else if (Input.GetKey(KeyCode.JoystickButton6))
                setJoystickText("JoystickButton6");
            else if (Input.GetKey(KeyCode.JoystickButton7))
                setJoystickText("JoystickButton7");
            else if (Input.GetKey(KeyCode.JoystickButton8))
                setJoystickText("JoystickButton8");
            else if (Input.GetKey(KeyCode.JoystickButton9))
                setJoystickText("JoystickButton9");
            else if (Input.GetKey(KeyCode.JoystickButton10))
                setJoystickText("JoystickButton10");
            else if (Input.GetKey(KeyCode.JoystickButton11))
                setJoystickText("JoystickButton11");


            else if (Input.GetButton("Fire1"))
                setJoystickText("Fire1");
            else if (Input.GetButton("Fire2"))
                setJoystickText("Fire2");
            else if (Input.GetButton("Fire3"))
                setJoystickText("Fire3");
            else if (Input.GetButton("Jump"))
                setJoystickText("Jump");
        }
        else
        {
        if (Input.GetAxis("Vertical") > 0)
            setJoystickText("V +");
        else if (Input.GetAxis("Vertical") < 0)
            setJoystickText("V -");

        if (Input.GetAxis("Horizontal") > 0)
            setJoystickText("H +");
        else if (Input.GetAxis("Horizontal") < 0)
            setJoystickText("H -");
        }
    }

    void setJoystickText (string printText)
    {
        joystickText.text = "Joystick: " + printText;
    }
}
