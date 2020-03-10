using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

// Class to implement and manage the Save and Load processes in the game
public class SaveHandler : MonoBehaviour
{
    private static string save_folder;

    /* Awake is called when the script instance is being loaded
    void Awake()
    {

#if UNITY_EDITOR
        save_folder = Application.dataPath + "/Saves";

#elif UNITY_ANDROID
        save_folder = Application.persistentDataPath + "/Saves";

#endif
        // Test if SAVE_FOLDER exists
        if (!Directory.Exists(save_folder))
        {
            // Create SAVE_FOLDER
            Directory.CreateDirectory(save_folder);
        }
    }
    */
    private void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_file = GameObject.FindGameObjectWithTag("GameFiles");       
        //save_folder = game_file.GetComponent<GameFilesHandler>().Saves_folder;
        save_folder = GameFilesHandler.Saves_folder;

        //GameFilesInitialization game_file_initialization = FindObjectOfType<GameFilesInitialization>();
        //save_folder = game_file_initialization.Saves_folder;
    }

    // Saves the structure information of the game (summarized in the SaveData class) into a Json file
    public void Save(string save_name)
    {
        string save_file = save_folder + save_name;

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

    // Loads a prevoius stored game from a Json file
    public void Load(string load_name)
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

    // Debug function
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
