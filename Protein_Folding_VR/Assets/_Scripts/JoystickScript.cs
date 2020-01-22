using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Prints the joystick input in the screen
public class JoystickScript : MonoBehaviour
{
    private string joystickMessage;

    public Text joystickText;

    private List<string> list;

    // Start is called before the first frame update
    void Start()
    {
        joystickMessage = "Starting!";
        setJoystickText(joystickMessage);
        list = new List<string> { "Fire1", "Fire2", "Fire3", "Jump", "A", "B", "C", "D", "Submit", "Cancel" };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            joystickMessage = Input.inputString;
            Debug.Log(joystickMessage);
            setJoystickText(joystickMessage);

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
                setJoystickText("Escape");

        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                setJoystickText("H " + Input.GetAxisRaw("Horizontal").ToString());

            if (Input.GetAxisRaw("Vertical") != 0)
                setJoystickText("V " + Input.GetAxisRaw("Vertical").ToString());

            if (Input.GetAxisRaw("Z-axis") != 0)
                setJoystickText("Z " + Input.GetAxisRaw("Z-axis").ToString());

            if (Input.GetButton("Fire1"))
                setJoystickText("Fire1");

        } 
        
         /*
        if (Input.GetAxisRaw("Horizontal") != 0)
            setJoystickText("H " + Input.GetAxisRaw("Horizontal").ToString());

        if (Input.GetAxisRaw("Vertical") != 0)
            setJoystickText("V " + Input.GetAxisRaw("Vertical").ToString());

        if (Input.GetAxisRaw("Z-axis") != 0)
            setJoystickText("Z " + Input.GetAxisRaw("Z-axis").ToString());

        foreach (var item in list)
            testJoystick(item);
        */
    }

    void testJoystick(string input)
    {
        if (Input.GetButton(input))
            setJoystickText(input);
    }

    void setJoystickText (string printText)
    {
        joystickText.text = "Joystick: " + printText;
    }
}
