using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the name file verification.
/// Calls the file save methods.
/// </summary>
public class SaveNewFileName : MonoBehaviour
{
    [SerializeField]
    private InputField input_text = default;    // Reference to Input Field object 

    private GameFilesHandler files_handler;     //Reference to Game Files Handler


    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// Verifies if the name choose to save file is valid.
    /// </summary>
    public void verifySaveName()
    {
        // Get the string in Input Field
        string file_name = input_text.text;
        
        // Tests
        // Empty name
        if (string.IsNullOrEmpty(file_name))
        {
            Debug.Log("Verify: Null or Empty");
            gameObject.GetComponent<NotifySaveNameError>().notifySaveNameError("Can't Save:\nSave name null or empty!");
        }
        // Name begins with white space
        else if (file_name[0].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in First");
            gameObject.GetComponent<NotifySaveNameError>().notifySaveNameError("Can't Save:\nSave name begins with whitespace!");
        }
        // Name ends with white space
        else if (file_name[file_name.Length - 1].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in Last");
            gameObject.GetComponent<NotifySaveNameError>().notifySaveNameError("Can't Save:\nSave name ends with whitespace!");
        }
        // Name already exists
        else if (files_handler.saveFileExists(file_name))
        {
            Debug.Log("Verify: Name already exists");
            gameObject.GetComponent<NotifySaveNameError>().notifySaveNameError("Can't Save:\nSave name already exists!");
        }
        // Valid name
        else
        {
            Debug.Log("Verify: " + file_name + " OK");
            files_handler.saveGame(file_name);

            // Save an output file
            files_handler.saveOutput(file_name);
            
            // Verify if the file was successfully saved
            if (files_handler.saveFileExists(file_name))
            {
                string file_path = Path.Combine(GameFilesHandler.Saves_folder, file_name + ".json");
                PlayerPrefs.SetString(GameFilesHandler.Saved_game, file_path);
                Debug.Log(file_path + "\nSave successful!");
                gameObject.GetComponent<NotifySave>().notifySave(file_name + "\nSave Successful!");
            }
            else
            {
                gameObject.GetComponent<NotifySave>().notifySave(file_name + "\nCouldn't be saved!");
            }

            // Verify if the output file was successfully saved
            if (files_handler.outputFileExists(file_name))
            {
                string file_path = Path.Combine(GameFilesHandler.Outputs_folder, file_name + ".txt");
                Debug.Log(file_path + "\nOutput successful!");               
            }
            else
            {
                Debug.Log(file_name + "\nCouldn't be saved in Outputs!");
            }
        }       
    }
}
