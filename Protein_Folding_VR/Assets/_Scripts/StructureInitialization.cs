using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System;
using System.Globalization;

using UnityEngine.Networking;

public class StructureInitialization : MonoBehaviour
{
    public static int n_mol;
    public GameObject residue;                 // Residue Prefab refernce
    public GameObject bond;                     // Bond Prefab reference
    public GameObject first;                    // First Prefab reference
    public Transform residues;                 // Instatitated Prefab reference
    public Transform bonds;                     // Instatitated Prefab reference
    public Transform first_ref;                 // Instatitated Prefab reference
    public static GameObject[] residues_structure;
    public static GameObject[] bonds_structure;
    public static GameObject first_mol;
    public static Vector3[] residues_coords;
    public static Quaternion[] residues_rotations;
    public static Vector3[] bonds_coords;
    public static Quaternion[] bonds_rotations;
    public static Vector3 pos_offset;
    public static string sequence;

    // Start is called before the first frame update.
    void Start()
    {
        string file_name;
        string read_data;

        // Check if the file with the protein structure was loaded in the MenuScene
        if (PlayerPrefs.HasKey("File_Name"))
        {
            file_name = PlayerPrefs.GetString("File_Name");
            //file_name = "13_teste_bestCoordinates.txt";

            // Verify if it is a new or a loaded game, by the file format
            // New game
            if (file_name.Contains(".txt")) // New game
            {
                // Read data in a Txt file to a string
                read_data = readTxtFile(file_name);

                // Check the string generated and loads only the necessary data to build a structure
                if (!String.IsNullOrEmpty(read_data))
                    loadInput(read_data);

                buildStructure();
            }
            // Load game
            else if (file_name.Contains(".json"))
            {
                gameObject.GetComponent<SaveHandler>().Load(file_name);
                //buildStructure();
                loadStructure();
            }
            // Error
            else
            {
                Debug.Log("StrucInit Error: Invalid File_Name!");
            }
        }
        else
        {
            Debug.Log("StructureInit Error: No File Loaded!");
        }
    }


    //Reads an input Text file into a string
    string readTxtFile(string file_name)
    {
        Debug.Log("StructureInitialization.readTxtFile()");
        // Loads the a file path in a special location that can be accessed in the application
        // More information search for "Streaming Assets"
        //var file_path = Application.streamingAssetsPath + "/Inputs/" + file_name;

        Debug.Log("File path: " + file_name);

#if UNITY_EDITOR

        if (File.Exists(file_name))
        {
            string read_data = File.ReadAllText(file_name);
            return read_data;
        }
        else
        {
            Debug.Log("Read Input: File not found!");
            return null;
        }

#elif UNITY_ANDROID

        UnityWebRequest webRequest = UnityWebRequest.Get(file_name);
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
        }
        return webRequest.downloadHandler.text;
      
#endif
    }

    // Load the input file data to the structure attributes
    void loadInput(string read_data)
    { 
        Debug.Log("StructureInitialization.loadInput()");
        // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array.
        string[] words = read_data.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        n_mol = int.Parse(words[Array.IndexOf(words, "Molecules") + 2]);
        Debug.Log("n_mol (Molecules) : " + n_mol);

        sequence = words[Array.IndexOf(words, "Sequence") + 2];
        Debug.Log("Sequence: " + sequence);

        // Takes the offset from the center (0.0 x , 0.0 y , 0.0 z) of the simulation box
        pos_offset.x = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.y = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.z = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
        Debug.Log("Position offset: " + $"<{pos_offset}>");

        // Initialize the structure in the center of the Scene with the first residue in Vector3(0, 0, 0)
        residues_coords = new Vector3[n_mol];
        for (var i = 0; i < n_mol * 4; i += 4)
        {
            var j = i / 4;
            residues_coords[j].x = float.Parse(words[i + 1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x;
            residues_coords[j].y = float.Parse(words[i + 2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y;
            residues_coords[j].z = float.Parse(words[i + 3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z;
        }
    }


    // Use to build a structure from data read from a file
    public void buildStructure()
    {
        residues_structure = new GameObject[n_mol];
        bonds_structure = new GameObject[n_mol - 1];
        bonds_coords = new Vector3[n_mol - 1];
        bonds_rotations = new Quaternion[n_mol - 1];
        initializeResidues();
        initializeBonds();
        asignResiduesJoints();
        asignBondsJoints();
        asignResidueColor();
        markFirstResidue();
    }

    // Use to destroy a structure, before the loading of a new or a saved one
    public void destroyStructure()
    {
        // Destroy the objects thal will be loaded
        for (var i = 0; i < n_mol - 1; i++)
        {
            Destroy(residues_structure[i]);
            Destroy(bonds_structure[i]);
        }
        Destroy(residues_structure[n_mol - 1]);
        Destroy(first_mol);
    }

    public void loadStructure()
    {
        residues_structure = new GameObject[n_mol];
        bonds_structure = new GameObject[n_mol - 1];
        //bonds_coords = new Vector3[n_mol - 1];
        //bonds_rotations = new Quaternion[n_mol - 1];
        loadResidues();
        loadBonds();
        asignResiduesJoints();
        asignBondsJoints();
        asignResidueColor();
        markFirstResidue();
    }

    // Initializes residues position
    void initializeResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            residues_structure[i] = Instantiate(residue, residues_coords[i], Quaternion.identity, residues);
            residues_structure[i].name = "Residue" + i.ToString();
        }
    }

    void loadResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            residues_structure[i] = Instantiate(residue, residues_coords[i], residues_rotations[i], residues);
            residues_structure[i].name = "Residue" + i.ToString();
        }
    }

    // Initializes bonds position.
    void initializeBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {
            // Define the mean position between a residue and its neighbour to place the bond coordinates.
            bonds_coords[i] = (residues_coords[i] + residues_coords[i + 1]) / 2;
            bonds_rotations[i] = Quaternion.FromToRotation(Vector3.down, residues_coords[i + 1] - residues_coords[i]);
            bonds_structure[i] = Instantiate(bond, bonds_coords[i], bonds_rotations[i], bonds);
            bonds_structure[i].name = "Bond" + i.ToString();
        }      
    }

    void loadBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {          
            bonds_structure[i] = Instantiate(bond, bonds_coords[i], bonds_rotations[i], bonds);
            bonds_structure[i].name = "Bond" + i.ToString();
        }
    }


    void asignResiduesJoints()
    {
        // The first residue ([0]) is conneced to the world origin coordinate.
        // The other residues are connected to the bond that precede them.
        for (var i = 1; i < n_mol; i++)
        {
            residues_structure[i].GetComponent<FixedJoint>().connectedBody = bonds_structure[i - 1].GetComponent<Rigidbody>();
        }
    }

    void asignBondsJoints()
    {
        // The first bond is connected to the first residue.
        bonds_structure[0].GetComponent<ConfigurableJoint>().connectedBody = residues_structure[0].GetComponent<Rigidbody>();

        // The other bonds are connected to the bond that precede them.
        for (var i = 1; i < n_mol - 1; i++)
        {
            bonds_structure[i].GetComponent<ConfigurableJoint>().connectedBody = bonds_structure[i - 1].GetComponent<Rigidbody>();
        }
    }

    void asignResidueColor()
    {
        for (var i = 0; i < n_mol; i++)
        {
            if (sequence[i] == 'A')
            {
                // Initialize the hydrophobic residue color
                residues_structure[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                // Initialize the polar residue color
                residues_structure[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    void markFirstResidue()
    {
        first_mol = Instantiate(first, residues_coords[0], Quaternion.identity, first_ref);
        first_mol.name = "First0";
    }
}
