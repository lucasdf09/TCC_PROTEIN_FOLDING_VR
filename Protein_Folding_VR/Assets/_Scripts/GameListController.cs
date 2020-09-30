using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Creates a list of buttons, with the files that can be loaded.
/// </summary>
public class GameListController : MonoBehaviour
{
    [SerializeField]
    private GameObject list_button = default;           // Button prefab reference

    [SerializeField]
    private GameObject message_text = default;          // View panel message object

    //[SerializeField]
    //private GameFilesHandler files_handler = default;   // Reference to GameFilesHandler
    private GameFilesHandler files_handler;             // Reference to GameFilesHandler

    [SerializeField]
    private int type = default;                         // Folder to list (Inputs or Saves)

    private List<GameObject> buttons_list;              //Buttons list

    private void Awake()
    {
        buttons_list = new List<GameObject>();
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
        Debug.Log("GameListController: Game Files Handler Found");   
    }

    /*
    // Start is called before the first frame update
    private void Start()
    {
        if (files_handler != default)
        {
            // Get the reference to the GameFilesHandler game object
            GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
            files_handler = game_files.GetComponent<GameFilesHandler>();
            Debug.Log("GameListController: Game Files Handler Found");
            //buttons_list = new List<GameObject>();
            //generateList();
        }       
    }
    */

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

        // Set the Folder to list: Inputs or Saves folder
        string[] file_names;
        string file_path;
        switch (type)
        {
            // Inputs folder
            case 1:
                file_path = GameFilesHandler.Inputs_folder;
                file_names = files_handler.readInputsFolder();
                //file_names = verifyInputFiles(file_names);
                message_text.GetComponent<Text>().text = "Input files not found!";
                break;

            // Saves folder
            case 2:
                file_path = GameFilesHandler.Saves_folder;
                file_names = files_handler.readSavesFolder();
                message_text.GetComponent<Text>().text = "Saved files not found!";
                break;

            default:
                file_path = null;
                file_names = null;
                Debug.Log("Invalid Type!");
                break;
        }

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
            Debug.Log("Sorted Names:");
            foreach (string name in sorted_names)
            {
                Debug.Log(name);
            }
            file_names = sorted_names;

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

    //**********************************************************************************
    /// <summary>
    /// Filter files that are not in the proper format for a New Game.
    /// </summary>
    /// <param name="file_names"></param>
    /// <returns></returns>
    private string[] verifyInputFiles(string[] file_names)
    {
        List<string> valid_names = new List<string>(); ;
        Debug.Log("Verify Input Files:");
        foreach (string name in file_names)
        {
            string file_path = Path.Combine(GameFilesHandler.Inputs_folder, name);
            Debug.Log(file_path);
            string read_data = files_handler.readTxtFile(name);
            bool isValid = false;

            if (!string.IsNullOrEmpty(read_data))
            {
                // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array
                string[] words = read_data.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int n_mol;
                string sequence;            
                // Check if Molecules mark exist
                if (words.Contains("Molecules"))
                { 
                    // Check if Molecules has an integer associated
                    if (int.TryParse(words[Array.IndexOf(words, "Molecules") + 2], out n_mol))
                    {
                        // Check if the integer is positive
                        if (n_mol > 0)
                        {
                            // Check if Sequence mark exists
                            if (words.Contains("Sequence"))
                            {                                
                                sequence = words[Array.IndexOf(words, "Sequence") + 2];
                                // Check if Sequence has a string associated
                                if (!string.IsNullOrEmpty(sequence))
                                {
                                    // Check if the sequence has the same size as molecules
                                    if (sequence.Length == n_mol)
                                    {
                                        // Check if the string has only As and Bs characters
                                        if (sequence.All(character => character == 'A' || character == 'B'))
                                        {
                                            Vector3[] coordinates = new Vector3[n_mol];
                                            // Check if the residues coordinates are in a valid format
                                            for (var i = 0; i < n_mol; i++)
                                            {
                                                // Check if the residue identificator is an integer
                                                if(int.TryParse(words[i * 4], out int residue))
                                                {
                                                    // Check if the identificator is in ascending order
                                                    if (residue == i)
                                                    {
                                                        // Check if the x coordinate is a float
                                                        if(float.TryParse(words[i * 4 + 1], out float x))
                                                        {
                                                            // Check if the y coordinate is a float
                                                            if (float.TryParse(words[i * 4 + 2], out float y))
                                                            {
                                                                // Check if the z coordinate is a float
                                                                if (float.TryParse(words[i * 4 + 3], out float z))
                                                                {
                                                                    // Check if the coordinate already exists
                                                                    if (!coordinates.Contains(new Vector3(x, y, z)))
                                                                    {
                                                                        coordinates[i] = new Vector3(x, y, z);
                                                                        // The first residue don't need to be checked
                                                                        if (i > 0)
                                                                        {
                                                                            var distance = Vector3.Distance(coordinates[i], coordinates[i - 1]);
                                                                            // Check if te value has a distance from the neighbour between 0.9 and 1.1
                                                                            if (distance > 0.9 && distance < 1.1)
                                                                            {
                                                                                isValid = true;
                                                                            }

                                                                        }
                                                                    }                                                                   
                                                                }
                                                            }                                                                                                                            
                                                        }                                                            
                                                    }
                                                }
                                                if (!isValid)
                                                {
                                                    break;
                                                }
                                                isValid = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }                        
                }          
            }
            valid_names.Add(name);
        }
        throw new NotImplementedException();
    }
    //**********************************************************************************

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
