using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

// Class to implement and manage the Save and Load processes in the game
public class SaveHandler : MonoBehaviour
{
    private static string save_folder;

    void Awake()
    {

#if UNITY_EDITOR
        save_folder = Application.dataPath + "/_Saves";

#elif UNITY_ANDROID
        SAVE_FOLDER = Application.persistentDataPath + "/_Saves";

#endif
        // Test if SAVE_FOLDER exists
        if (!Directory.Exists(save_folder))
        {
            // Create SAVE_FOLDER
            Directory.CreateDirectory(save_folder);
        }
    }

    public void Save()
    {
        Debug.Log("SaveHandler Saving!");

        // Save the game state
        SaveData save_slot = new SaveData();
        save_slot.n_mol = StructureInitialization.n_mol;
        save_slot.sequence = StructureInitialization.sequence;
        save_slot.residues_coords = new Vector3[save_slot.n_mol];
        for (var i = 0; i < save_slot.n_mol; i++)
        {
            save_slot.residues_coords[i] = StructureInitialization.res_coords[i];
        }

        printSaveData("SAVE", save_slot);

        // Convert to Json format
        string json = JsonUtility.ToJson(save_slot);

        Debug.Log(json);

        string save_name = save_folder + "/saveTest.json";

        Debug.Log("Data path: " + save_name);
        File.WriteAllText(save_name, json);
    }


    public void Load()
    {
        string load_name = save_folder + "/saveTest.json";
        if (File.Exists(load_name))
        {
            string json = File.ReadAllText(load_name);
            Debug.Log("SaveHandler Loading!");
            SaveData load_slot = new SaveData();
            load_slot = JsonUtility.FromJson<SaveData>(json);

            printSaveData("LOAD", load_slot);
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
