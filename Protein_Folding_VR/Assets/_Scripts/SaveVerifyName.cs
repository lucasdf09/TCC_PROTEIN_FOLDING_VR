using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class SaveVerifyName : MonoBehaviour
{
    [SerializeField]
    private InputField input_text;

    private static GameFilesHandler game_files_handler;


    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        game_files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    public void verifySaveName()
    {
        string file_name = input_text.text;
        // Empty name
        if (string.IsNullOrEmpty(file_name))
        {
            Debug.Log("Verify: Null or Empty");
        }
        // All white space name
        else if (string.IsNullOrWhiteSpace(file_name))
        {
            Debug.Log("Verify: Null or WhiteSpace");
        }
        // Name begins with white space
        else if (file_name[0].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in First");
        }
        // Name ends with white space
        else if (file_name[file_name.Length - 1].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in Last");
        }
        // Name already exists
        else if (saveFileExists(file_name))
        {
            Debug.Log("Verify: Name already exists");
        }
        // Valid name
        else
        {
            Debug.Log("Verify: " + file_name + " OK");
            //game_files_handler.saveGame(file_name);

        }
        
    }

    private bool saveFileExists(string file_name)
    {
        string [] files = game_files_handler.readSavesFolder();
        Debug.Log("saveFileExists:\n");
        foreach (string file in files)
        {
            Debug.Log(file);
            if (file.Equals(file_name + ".json"))
            {
                return true;
            }
        }      
        return false;
    }
}
