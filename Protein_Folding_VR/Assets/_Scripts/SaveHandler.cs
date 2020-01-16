using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

// Class to implement and manage the Save and Load processes in the game
public class SaveHandler : MonoBehaviour
{
    private static string save_folder;

    // Awake is called when the script instance is being loaded
    void Awake()
    {

#if UNITY_EDITOR
        save_folder = Application.dataPath + "/Saves";

#elif UNITY_ANDROID
        SAVE_FOLDER = Application.persistentDataPath + "/Saves";

#endif
        // Test if SAVE_FOLDER exists
        if (!Directory.Exists(save_folder))
        {
            // Create SAVE_FOLDER
            Directory.CreateDirectory(save_folder);
        }
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
        save_slot.residues_coords = new Vector3[save_slot.n_mol];
        for (var i = 0; i < save_slot.n_mol; i++)
        {
            save_slot.residues_coords[i] = StructureInitialization.res_structure[i].transform.position;
        }

        // Debug stuff
        printSaveData("SAVE", save_slot);

        // Convert to Json format
        string json = JsonUtility.ToJson(save_slot);

        // Debug stuff
        Debug.Log(json);

        File.WriteAllText(save_file, json);
    }

    // Loads a prevoius stored game from a Json file
    public void Load(string load_name)
    {
        string load_file = save_folder + load_name;

        Debug.Log("SaveHandler Loading!");
        Debug.Log("Load file: " + load_file);

        if (File.Exists(load_file))
        {
            string json = File.ReadAllText(load_file);          
            SaveData load_slot = new SaveData();
            load_slot = JsonUtility.FromJson<SaveData>(json);

            // Debug stuff
            printSaveData("LOAD", load_slot);

            StructureInitialization.n_mol = load_slot.n_mol;
            StructureInitialization.sequence = load_slot.sequence;
            StructureInitialization.res_coords = new Vector3[load_slot.n_mol];
            for (var i = 0; i < load_slot.n_mol; i++)
            {
                StructureInitialization.res_coords[i] = load_slot.residues_coords[i];
            }
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
        for (var i = 0; i < slot.n_mol; i++)
        {
            Debug.Log(i + ": " + slot.residues_coords[i]);
        }
    }
 }
