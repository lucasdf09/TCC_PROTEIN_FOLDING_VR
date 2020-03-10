using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.Networking;
using System;

/// <summary>
/// Sets the directory path names used in the game.
/// Verifies and creates the directories used in game.
/// </summary>
public class GameFilesHandler : MonoBehaviour
{
    //  *** PlayerPrefs string names ***
    private static string new_game;   // Key associated with new game files from inputs folder
    private static string saved_game;    // Key associated with saved game files from saves folder

    //  ***The folders used to manipulate user data ***
    private static string inputs_folder;    //  Folder to store the files of raw structures
    private static string outputs_folder;   //  Folder to store the files of structures to be exported
    private static string saves_folder;     //  Folder to store the files of saved structures

    //  *** PlayerPrefs names properties ***
    /// <summary>
    /// Returns the new game PlayerPrefs key string
    /// </summary>
    public static string New_game
    {
        get
        {
            return new_game;
        }
    }

    /// <summary>
    /// Returns the saved game PlayerPrefs key string
    /// </summary>
    public static string Saved_game
    {
        get
        {
            return saved_game;
        }
    }

    //  *** Folders attributes properties ***
    /// <summary>
    /// Returns the folder with files of the raw structures
    /// </summary>
    public static string Inputs_folder
    {
        get
        {
            return inputs_folder;
        }
    }

    /// <summary>
    /// Returns the folder with files of the structures to be exported
    /// </summary>
    public static string Outputs_folder
    {
        get
        {
            return outputs_folder;
        }
    }

    /// <summary>
    /// Returns the folder with files of the saved structures
    /// </summary>
    public static string Saves_folder
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
        // Persistence of the GameFilesHandler GameObject
        DontDestroyOnLoad(this.gameObject);

        //*** Game Files PlayerPrefs initialization ***       
        
        // Key strings initialization
        new_game = "New_Game";
        saved_game = "Saved_Game";
        // PlayerPrefs fields initialization
        PlayerPrefs.SetString(new_game, null);
        PlayerPrefs.SetString(saved_game, null);

        //  *** Game Files Folders initialization ***

        // Intermediary folder to contain the game's files
        string game_files_folder = "_GameFiles";

#if UNITY_EDITOR
        game_files_folder = Path.Combine(Application.dataPath, game_files_folder);

#elif UNITY_ANDROID
        game_files_folder = Path.Combine(Application.persistentDataPath, game_files_folder);
#endif

        // Set the folder names 
        inputs_folder = Path.Combine(game_files_folder, "Inputs");
        outputs_folder = Path.Combine(game_files_folder, "Outputs");
        saves_folder = Path.Combine(game_files_folder, "Saves");

        // Verify the folders existance
        folderCheck(game_files_folder);
        folderCheck(inputs_folder);
        folderCheck(outputs_folder);
        folderCheck(saves_folder);

        // Check if files in Streaming Assets have already been copied to the Inputs folder
        // Read the string with the names of the pre-loaded structures
        string string_file = readFromStreamingAssets("Input_File_Names.txt");
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

        // Check if there are files in Inputs folder
        DirectoryInfo inputs_directory = new DirectoryInfo(inputs_folder);
        //FileInfo[] info = inputs_directory.GetFiles("*.*");
        FileInfo[] info = inputs_directory.GetFiles("*.txt");
        Debug.Log("Inputs size: " + info.Length);
        
        // Debug stuff
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
        }

        // IF Inputs folder empty: copy the standard new game structures to Inputs folder
        if (info.Length == 0)
        {
            Debug.Log("Inputs empty!");

            // Copy the files in StreamingAssets > Inputs to GameFiles > Inputs 
            foreach (string input in input_files)
            {
                copyToInputs(input);
                /*
                string new_input_file = readFromStreamingAssets(input);
                //Debug.Log(new_input_file);
                string new_input_file_path = Path.Combine(inputs_folder, input);

                Debug.Log("Saving: " + new_input_file_path + " in " + inputs_folder);

                File.WriteAllText(new_input_file_path, new_input_file);
                */
            }
        }
        
        // If Inputs folder not empty: verify if the standards new game structures exists
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
                    copyToInputs(input);
                    /*
                    // Copy input to GameFile > Inputs folder                 
                    string new_input_file = readFromStreamingAssets(input);
                    //Debug.Log(new_input_file);
                    string new_input_file_path = Path.Combine(inputs_folder, input);

                    Debug.Log("Saving: " + new_input_file_path + " in " + inputs_folder);

                    File.WriteAllText(new_input_file_path, new_input_file);
                    */
                }
            }
        }
        Debug.Log("GameFilesHandler END!!!");
    }

    /// <summary>
    /// Copy a file to Inputs folder
    /// </summary>
    /// <param name="file">File name only</param>
    private void copyToInputs(string file)
    {
        // Copy input to GameFile > Inputs folder                 
        string new_input_file = readFromStreamingAssets(file);
        //Debug.Log(new_input_file);
        string new_input_file_path = Path.Combine(inputs_folder, file);

        Debug.Log("Saving: " + new_input_file_path + " in " + inputs_folder);

        File.WriteAllText(new_input_file_path, new_input_file);
    }

    /// <summary>
    /// Reads an input Text file into a string, only for files in StreamingAssets > Inputs folder
    /// </summary>
    /// <param name="file_path">File name to be read in StreamingAssets > Inputs folder</param>
    /// <returns>File content</returns>
    private string readFromStreamingAssets(string file_name)
    {
        string file_path = Path.Combine(Application.streamingAssetsPath, "Inputs", file_name);
        Debug.Log("Reading From Streaming Assets");
    
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

        // Loads the file path in a special location that can be accessed in the application
        // More information search for "Streaming Assets"
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

    /// <summary>
    /// Checks if folder exists and creates it, if it hasn't been done yet
    /// </summary>
    /// <param name="folder_path">Folder path</param>
    private void folderCheck(string folder_path)
    {
        // Test if files folders exists. Create then if necessary.
        if (!Directory.Exists(folder_path))
        {
            // Create GameFiles
            Directory.CreateDirectory(folder_path);
            Debug.Log("CREATED: " + folder_path);
        }
        else
        {
            Debug.Log("Already exists: " + folder_path);
        }
    }

    //  *** Functions used in game ***

    //  *** Public methods to access the game files directories ***
    
    /// <summary>
    /// Returns the Inputs folder contents
    /// </summary>
    /// <returns>Files paths</returns>
    public string[] readInputsFolder()
    {
        return readDirectory(inputs_folder, "*.txt");
    }

    /// <summary>
    /// Returns the Outputs folder contents
    /// </summary>
    /// <returns>Files paths</returns>
    public string[] readOutputsFolder()
    {
        return readDirectory(outputs_folder, "*.txt");
    }

    /// <summary>
    /// Returns the Saves folder contents
    /// </summary>
    /// <returns>Files paths</returns>
    public string[] readSavesFolder()
    {
        return readDirectory(saves_folder, "*.json");
    }

    /// <summary>
    /// Reads a directory and returns a list of files with type extension
    /// </summary>
    /// <param name="path">Directory path</param>
    /// <param name="extension">Type of file to be listed</param>
    /// <returns></returns>
    private string[] readDirectory(string path, string extension)
    {
        Debug.Log("GameFilesHandler path: " + path);
        DirectoryInfo directory = new DirectoryInfo(path);
        //FileInfo[] info = inputs_directory.GetFiles("*.*");
        FileInfo[] files = directory.GetFiles(extension);

        Debug.Log("GameFilesHandler: Directory size: " + files.Length);
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

    /// <summary>
    /// Saves the actual structure information of the game (summarized in the SaveData class) into a Json file
    /// </summary>
    /// <param name="save_name">File name without extension</param>
    public void saveGame(string save_name)
    {
        string save_file = Path.Combine(saves_folder, save_name, ".json");

        Debug.Log("SaveHandler Saving!");
        Debug.Log("Save file: " + save_file);

        // Save the game state
        SaveData save_slot = new SaveData();
        save_slot.n_mol = StructureInitialization.n_mol;
        save_slot.sequence = StructureInitialization.sequence;
        save_slot.best_energy = PlayerController.best_energy;
        save_slot.score = PlayerController.score;
        save_slot.residues_coords = new Vector3[save_slot.n_mol];
        save_slot.residues_rotations = new Quaternion[save_slot.n_mol];
        save_slot.bonds_coords = new Vector3[save_slot.n_mol - 1];
        save_slot.bonds_rotations = new Quaternion[save_slot.n_mol - 1];
        for (var i = 0; i < save_slot.n_mol - 1; i++)
        {
            save_slot.residues_coords[i] = StructureInitialization.residues_structure[i].transform.position;
            save_slot.residues_rotations[i] = StructureInitialization.residues_structure[i].transform.rotation;
            save_slot.bonds_coords[i] = StructureInitialization.bonds_structure[i].transform.position;
            save_slot.bonds_rotations[i] = StructureInitialization.bonds_structure[i].transform.rotation;
        }
        save_slot.residues_coords[save_slot.n_mol - 1] = StructureInitialization.residues_structure[save_slot.n_mol - 1].transform.position;
        save_slot.residues_rotations[save_slot.n_mol - 1] = StructureInitialization.residues_structure[save_slot.n_mol - 1].transform.rotation;

        // Convert to Json format
        string json = JsonUtility.ToJson(save_slot);

        // Write Json string to a file
        File.WriteAllText(save_file, json);


        // DEBUG
        //printSaveData("SAVE", save_slot);
        //Debug.Log(json);
    }

    /// <summary>
    /// Loads a stored game from a Json file
    /// </summary>
    /// <param name="load_name">File path</param>
    public void loadGame(string load_name)
    {
        //string load_file = save_folder + load_name;
        string load_file = load_name;

        Debug.Log("SaveHandler Loading!");
        Debug.Log("Load file: " + load_file);

        if (File.Exists(load_file))
        {
            string json = File.ReadAllText(load_file);
            SaveData load_slot = new SaveData();
            load_slot = JsonUtility.FromJson<SaveData>(json);

            // DEBUG
            // printSaveData("LOAD", load_slot);

            StructureInitialization.n_mol = load_slot.n_mol;
            StructureInitialization.sequence = load_slot.sequence;
            PlayerController.best_energy = load_slot.best_energy;
            PlayerController.saved_score = load_slot.score;
            StructureInitialization.residues_coords = new Vector3[load_slot.n_mol];
            StructureInitialization.residues_rotations = new Quaternion[load_slot.n_mol];
            StructureInitialization.bonds_coords = new Vector3[load_slot.n_mol - 1];
            StructureInitialization.bonds_rotations = new Quaternion[load_slot.n_mol - 1];
            for (var i = 0; i < load_slot.n_mol - 1; i++)
            {
                StructureInitialization.residues_coords[i] = load_slot.residues_coords[i];
                StructureInitialization.residues_rotations[i] = load_slot.residues_rotations[i];
                StructureInitialization.bonds_coords[i] = load_slot.bonds_coords[i];
                StructureInitialization.bonds_rotations[i] = load_slot.bonds_rotations[i];
            }
            StructureInitialization.residues_coords[load_slot.n_mol - 1] = load_slot.residues_coords[load_slot.n_mol - 1];
            StructureInitialization.residues_rotations[load_slot.n_mol - 1] = load_slot.residues_rotations[load_slot.n_mol - 1];
        }
        else
        {
            Debug.Log("LOAD: File not found!");
        }
    }

    //  *** Debug function ***
    /// <summary>
    /// Print info about the save/load operations
    /// </summary>
    /// <param name="operation">Save or load</param>
    /// <param name="slot">Saved data reference object</param>
    private void printSaveData(string operation, SaveData slot)
    {
        Debug.Log(operation);
        Debug.Log(slot.n_mol);
        Debug.Log(slot.sequence);
        Debug.Log(slot.best_energy);
        Debug.Log(slot.score);
        for (var i = 0; i < slot.n_mol - 1; i++)
        {
            Debug.Log(i + ": " + slot.residues_coords[i]);
            Debug.Log(i + ": " + slot.residues_rotations[i]);
            Debug.Log(i + ": " + slot.bonds_coords[i]);
            Debug.Log(i + ": " + slot.bonds_rotations[i]);
        }
        Debug.Log((slot.n_mol - 1) + ": " + slot.residues_coords[slot.n_mol - 1]);
        Debug.Log((slot.n_mol - 1) + ": " + slot.residues_rotations[slot.n_mol - 1]);
    }

}