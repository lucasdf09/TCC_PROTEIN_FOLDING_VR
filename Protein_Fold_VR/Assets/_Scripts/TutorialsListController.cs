using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Creates a list of buttons, with the tutorials files names.
/// </summary>
public class TutorialsListController : MonoBehaviour
{
    [SerializeField]
    private GameObject list_button = default;           // Button prefab reference

    [SerializeField]
    private GameObject message_text = default;          // View panel message object

    private GameFilesHandler files_handler;             // Reference to GameFilesHandler

    private List<GameObject> buttons_list;              //Buttons list

    private void Awake()
    {
        // Get the GameFilesHandler reference
        buttons_list = new List<GameObject>();
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
        Debug.Log("TutorialsListController: Game Files Handler Found");
    }

    /// <summary>
    /// Generates the buttons list.
    /// </summary>
    public void generateList()
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
    
        // List the Tutorials folder
        string[] file_names;
        string file_path;
        file_path = GameFilesHandler.Tutorials_folder;
        file_names = files_handler.readTutorialsFolder();
        message_text.GetComponent<Text>().text = "Tutorials files not found!";
      
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

            // Sort the array with the names alphanumerically
            //var sorted_names = file_names.OrderBy(file => file).ToArray();
            var sorted_names = file_names.OrderBy(file => PadNumbers(file)).ToArray();

            file_names = sorted_names;

            // Create buttons
            //Debug.Log("Buttons_list BUTTONS");
            foreach (string name in file_names)
            {
                //Debug.Log(name);
                GameObject button = Instantiate(list_button, list_button.transform.parent) as GameObject;
                button.SetActive(true);

                button.name = Path.GetFileNameWithoutExtension(name);

                button.GetComponent<GameListButton>().Button_text = Path.GetFileNameWithoutExtension(name);

                button.GetComponent<GameListButton>().Button_file = Path.Combine(file_path, name);

                buttons_list.Add(button);
            }
        }
    }

    /// <summary>
    /// Pads the numbers in a string, filling them with zeros until the number has 10 digits.
    /// </summary>
    /// <param name="input">String name.</param>
    /// <returns>String with numbers filled with zeros to the left.</returns>
    //public static string PadNumbers(string input)
    private string PadNumbers(string input)
    {
        return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
    }
}
