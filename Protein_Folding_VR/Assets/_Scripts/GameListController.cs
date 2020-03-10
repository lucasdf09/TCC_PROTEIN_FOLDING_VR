using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates the a list of buttons, with the files that can be loaded
/// </summary>
public class GameListController : MonoBehaviour
{
    [SerializeField]
    private GameObject list_button;         // Button prefab reference

    [SerializeField]
    private GameObject message_text;        // View panel message object

    [SerializeField]
    private GameFilesHandler game_files_handler;    // Reference to GameFilesHandler

    [SerializeField]
    private int type;       // Type of file to load (new or load game)

    private List<GameObject> buttons_list;  //Buttons list (NOT IN USE)

    // Start is called before the first frame update
    void Start()
    {
        buttons_list = new List<GameObject>();
        generateList();
    }

    /// <summary>
    /// Generates the buttons list
    /// </summary>
    private void generateList()
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
        string[] file_names;
        string file_path;
        //string file_extension;
        switch (type)
        {
            // New Game
            case 1:
                file_path = GameFilesHandler.Inputs_folder;
                //file_extension = "*.txt";
                file_names = game_files_handler.readInputsFolder();
                message_text.GetComponent<Text>().text = "Input files not found!";
                break;

            // Load Game
            case 2:
                file_path = GameFilesHandler.Saves_folder;
                //file_extension = "*.json";
                file_names = game_files_handler.readSavesFolder();
                message_text.GetComponent<Text>().text = "Saved files not found!";
                break;

            default:
                file_path = null;
                //file_extension = null;
                file_names = null;
                Debug.Log("Invalid Type!");
                break;
        }

        // Read Input_folder to load the new game files
        //file_names = game_files_handler.readDirectory(file_path, file_extension);

        Debug.Log("File_names Length: = " + file_names.Length);

        // Verify if there are files
        // If doesn't has files: show a message
        if (file_names.Length == 0)
        {
            message_text.SetActive(true);
        }

        // If has files: proceed with the generation list
        else
        {
            //Disable the missing files message
            message_text.SetActive(false);           
            
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

            // Debug stuff
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

        }
    }

}
