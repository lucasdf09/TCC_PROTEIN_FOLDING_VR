using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameListController : MonoBehaviour
{
    [SerializeField]
    private GameObject list_button;         // Button prefab reference

    [SerializeField]
    private GameObject message_text;

    //[SerializeField]
    //private int[] int_array;

    //[SerializeField]
    //private GameObject GameFilesHandler;

    [SerializeField]
    private GameFilesInitialization game_files_handler;

    [SerializeField]
    private int type;

    private List<GameObject> buttons_list;


    // Start is called before the first frame update
    void Start()
    {
        buttons_list = new List<GameObject>();
        generateList();
    }

    void generateList()
    {
        // If the list is not empty
        if (buttons_list.Count > 0)
        {
            // Clear the list of buttons to generate a new one
            foreach (GameObject button in buttons_list)
            {
                Destroy(button.gameObject);
            }
            buttons_list.Clear();
        }

        // Set the type of operation that will be made: New or Load Game
        string file_path;
        string file_extension;
        switch (type)
        {
            case 1:
                file_path = game_files_handler.Inputs_folder;
                file_extension = "*.txt";
                message_text.GetComponent<Text>().text = "Input files not found!";
                break;

            case 2:
                file_path = game_files_handler.Saves_folder;
                file_extension = "*.json";
                message_text.GetComponent<Text>().text = "Saved files not found!";
                break;

            default:
                file_path = null;
                file_extension = null;
                Debug.Log("Invalid Type!");
                break;
        }

        // Read Input_folder to load the new game files
        string[] file_names = game_files_handler.readDirectory(file_path, file_extension);

        Debug.Log("File_names Length: = " + file_names.Length);

        if (file_names.Length == 0)
        {
            message_text.SetActive(true);
        }

        else
        {
            // Sort file_names array
            // MISSING!!!

            // Create buttons
            Debug.Log("Buttons_list BUTTONS");
            foreach (string name in file_names)
            {
                Debug.Log(name);
                //GameObject button = Instantiate(list_button) as GameObject;
                GameObject button = Instantiate(list_button, list_button.transform.parent) as GameObject;
                button.SetActive(true);

                //Debug.Log("Name only: " + name.Split('.')[0]);
                //Debug.Log("Name : " + Path.GetFileNameWithoutExtension(name));

                button.name = Path.GetFileNameWithoutExtension(name);

                //button.GetComponent<GameListButton>().setText(name);
                button.GetComponent<GameListButton>().Button_text = Path.GetFileNameWithoutExtension(name);

                button.GetComponent<GameListButton>().Button_file = Path.Combine(file_path, name);

                //button.transform.SetParent(list_button.transform.parent, false);

                buttons_list.Add(button);
            }

            Debug.Log("Buttons names:");
            foreach (GameObject button in buttons_list)
            {
                Debug.Log(button.name);
            }

            Debug.Log("Buttons Files:");
            foreach (GameObject button in buttons_list)
            {
                Debug.Log(button.GetComponent<GameListButton>().Button_file);
            }

            /*
            foreach(int i in int_array)
            {
                //GameObject button = Instantiate(list_button) as GameObject;
                GameObject button = Instantiate(list_button, list_button.transform.parent) as GameObject;
                button.SetActive(true);

                button.name = "ListButton_" + i;

                //Debug.Log("Initialized: Button " + i);
                button.GetComponent<GameListButton>().setText("Button " + i);

                //button.transform.SetParent(list_button.transform.parent, false);
            }
            */

        }
    }

    /*
    // Reads a directory and return its .txt files names list
    private string[] readDirectory(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        //FileInfo[] info = inputs_directory.GetFiles("*.*");
        FileInfo[] files = directory.GetFiles("*.txt");

        Debug.Log("GameListController: Inputs size: " + files.Length);
        foreach (FileInfo file in files)
        {
            Debug.Log(file.Name);
        }
        string[] file_names = new string[files.Length];
        for(var i = 0; i < files.Length; i++)
        {
            file_names[i] = files[i].Name;
        }
        return file_names;      
    }

    
    public void buttonClicked(string string_text)
    {
        Debug.Log(string_text);

    }
    */
}
