using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.Networking;
using System;

public class GameFilesInitialization : MonoBehaviour
{
    private static string inputs_folder;
    private static string outputs_folder;
    private static string saves_folder;
   
    // "Folder" attributes properties
    public string Inputs_folder
    {
        get
        {
            return inputs_folder;
        }
    }
    public string Outputs_folder
    {
        get
        {
            return outputs_folder;
        }
    }   
    public string Saves_folder
    {
        get
        {
            return saves_folder;
        }
    }

    private void Awake()
    {
        // Remove other GameFiles that could be created when reloading the MainMenu scene
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameFiles");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        // Persistence of the GameFilesHandler GameObject.
        DontDestroyOnLoad(this.gameObject);

        // Folder to contain the game's files
        string game_files_folder = "_GameFiles";

#if UNITY_EDITOR
        game_files_folder = Path.Combine(Application.dataPath, game_files_folder);

#elif UNITY_ANDROID
        game_files_folder = Path.Combine(Application.persistentDataPath, game_files_folder);
#endif

        inputs_folder = Path.Combine(game_files_folder, "Inputs");
        outputs_folder = Path.Combine(game_files_folder, "Outputs");
        saves_folder = Path.Combine(game_files_folder, "Saves");

        folderCheck(game_files_folder);
        folderCheck(inputs_folder);
        folderCheck(outputs_folder);
        folderCheck(saves_folder);

        // Check whether files in Streaming Assets have already been copied to the Inputs folder

        string file_path = Path.Combine(Application.streamingAssetsPath, "Inputs");
        file_path = Path.Combine(file_path, "Input_File_Names.txt");

        // Read the string with the names of the pre-loaded structures
        string string_file = readTxtFile(file_path);
        Debug.Log(string_file);

        // Split the names of the input files in Input_File_Names to a string array
        string[] input_files = string_file.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("input_files size: " + input_files.Length);
        /*
        foreach (string input in input_files)
        {
            Debug.Log(input);
        }
        */

        // Check if there are files in Inputs
        DirectoryInfo inputs_directory = new DirectoryInfo(inputs_folder);
        //FileInfo[] info = inputs_directory.GetFiles("*.*");
        FileInfo[] info = inputs_directory.GetFiles("*.txt");
        Debug.Log("Inputs size: " + info.Length);
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
        }
        if (info.Length == 0)
        {
            Debug.Log("Inputs empty!");

            // Copy the files in StreamingAssets > Inputs to GameFiles > Inputs 
            foreach (string input in input_files)
            {
                string path = Path.Combine(Application.streamingAssetsPath, "Inputs");
                string new_input_file = readTxtFile(Path.Combine(path, input));
                //Debug.Log(new_input_file);
                string new_input_file_name = Path.Combine(inputs_folder, input);
                Debug.Log("Saving: " + new_input_file_name + " in " + inputs_folder);
                File.WriteAllText(new_input_file_name, new_input_file);
            }
        }
        else
        {
            Debug.Log("Inputs NOT empty!");

            // Verify if the inputs_folder has all the standard input files in StreamingAssets
            foreach (string input in input_files)
            {
                bool flag = false;
                Debug.Log("Searching: " + input);              
                foreach (FileInfo file in info)
                {
                    if (file.Name.Equals(input))
                    {
                        flag = true;
                        Debug.Log("Found: " + file.Name);
                    }
                }
                if (!flag)
                {
                    // Copy input to GameFile > Inputs folder

                    string path = Path.Combine(Application.streamingAssetsPath, "Inputs");
                    string new_input_file = readTxtFile(Path.Combine(path, input));
                    //Debug.Log(new_input_file);
                    string new_input_file_name = Path.Combine(inputs_folder, input);
                    Debug.Log("Saving: " + new_input_file_name + " in " + inputs_folder);
                    File.WriteAllText(new_input_file_name, new_input_file);
                }
            }
        }


        Debug.Log("Game File Initialization END!!!");
    }

    /// Reads an input Text file into a string, only for files in StreamingAssets folder
    string readTxtFile(string file_path)
    {
        Debug.Log("GameFileInitialization.readTxtFile()");
        // Loads the a file path in a special location that can be accessed in the application
        // More information search for "Streaming Assets"
        //var file_path = Path.Combine(Application.streamingAssetsPath, file_name);

        Debug.Log("File path: " + file_path);

#if UNITY_EDITOR
        try
        {
            string read_data = File.ReadAllText(file_path);
            return read_data;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return null;
        }
        
#elif UNITY_ANDROID
        try
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(file_path);
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
            }
            return webRequest.downloadHandler.text;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return null;
        }
#endif

    }


    /// Checks if the "folder" exists and creates it, if it hasn't been done yet
    private void folderCheck(string folder)
    {
        // Test if files folders exists. Create then if necessary.
        if (!Directory.Exists(folder))
        {
            // Create GameFiles
            Directory.CreateDirectory(folder);
            Debug.Log("CREATED: " + folder);
        }
        else
        {
            Debug.Log("Already exists: " + folder);
        }
    }


    // Reads a directory and return a list of files of type extension
    public string[] readDirectory(string path, string extension)
    {
        Debug.Log("GameFilesInit path: " + path);
        DirectoryInfo directory = new DirectoryInfo(path);
        //FileInfo[] info = inputs_directory.GetFiles("*.*");
        FileInfo[] files = directory.GetFiles(extension);

        Debug.Log("GameFilesInitialization: Inputs size: " + files.Length);
        foreach (FileInfo file in files)
        {
            Debug.Log(file.Name);
        }
        string[] file_names = new string[files.Length];
        for (var i = 0; i < files.Length; i++)
        {
            file_names[i] = files[i].Name;
        }
        return file_names;
    }
}