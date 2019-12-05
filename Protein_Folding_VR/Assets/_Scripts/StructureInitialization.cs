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
    public GameObject residue;                 // Residue Prefab refernce.
    public GameObject bond;                     // Bond Prefab reference.
    public GameObject first;                    // First Prefab reference.
    public Transform residues;                 // Instatitated Prefab reference.
    public Transform bonds;                     // Instatitated Prefab reference.
    public Transform first_ref;                 // Instatitated Prefab reference.
    public static GameObject[] res_structure;
    public static GameObject[] bond_structure;
    public static GameObject first_mol;
    public static Vector3[] res_coords;
    public static Vector3[] bond_coords;
    public static Quaternion[] bond_rotations;
    public static Vector3 pos_offset;
    public static string sequence;

    //public static bool select_mode;
    //public static bool move_mode;
    //public static GameObject first;
    //public static float scale;

    //public GameObject CubeTest;

    // Start is called before the first frame update.
    void Start()
    {
        // Loads the name of the file with the molecule structure to be builded.
        //string file_name = "Inputs/" + PlayerPrefs.GetString("File_Name");
        string file_name = "/Inputs/13_teste_bestCoordinates.txt";

        string read_data = readTxtInput(file_name);

        if (!String.IsNullOrEmpty(read_data))
            loadInput(read_data);

        res_structure = new GameObject[n_mol];
        bond_structure = new GameObject[n_mol - 1];
        bond_coords = new Vector3[n_mol - 1];
        bond_rotations = new Quaternion[n_mol - 1];
        PlayerController.select_mode = false;
        PlayerController.move_mode = false;
        //scale = 10.0f;
        initializeResidues();
        initializeBonds();
        asignResiduesJoints();
        asignBondsJoints();
        asignResidueColor();
        markFirstResidue();
        PlayerController.select_mode = true;        
    }

    //Reads the input file.
    string readTxtInput(string file_name)
    {
        // Loads the a file path in a special location that can be accessed in the application.
        // For More information, search for "Streaming Assets".
        //string file_path = Path.Combine(Application.streamingAssetsPath, file_name);

        var file_path = Application.streamingAssetsPath + file_name;

        Debug.Log("File path: " + file_path);

#if UNITY_EDITOR

        if (File.Exists(file_path))
        {
            string read_data = File.ReadAllText(file_path);
            return read_data;
        }
        else
        {
            Debug.Log("Read Input: File not found!");
            return null;
        }

#elif UNITY_ANDROID

        UnityWebRequest webRequest = UnityWebRequest.Get(file_path);
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
        }
        return webRequest.downloadHandler.text;
      
#endif

        /*
        // Loads all the TXT input file to "words[]".
        StreamReader reader = new StreamReader(file_path);
        string full_text = reader.ReadToEnd();
        reader.Close();
        */
    }

    void loadInput(string read_data)
    {
        // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array.
        string[] words = read_data.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


        //for (var i = 0; i < words.Length; i++)
        //{
        //    Debug.Log("words[" + i + "] = " + words[i]);
        //}

        n_mol = int.Parse(words[Array.IndexOf(words, "Molecules") + 2]);
        Debug.Log("n_mol (Molecules) : " + n_mol);

        sequence = words[Array.IndexOf(words, "Sequence") + 2];
        Debug.Log("Sequence: " + sequence);

        pos_offset.x = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.y = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.z = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);

        Debug.Log("Position offset: " + $"<{pos_offset}>");

        res_coords = new Vector3[n_mol];

        for (var i = 0; i < n_mol * 4; i += 4)
        {
            var j = i / 4;
            //residueCoords[j].x = (float.Parse(words[i+1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x) * scale;
            //residueCoords[j].y = (float.Parse(words[i+2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y) * scale;
            //residueCoords[j].z = (float.Parse(words[i+3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z) * scale;
            res_coords[j].x = float.Parse(words[i + 1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x;
            res_coords[j].y = float.Parse(words[i + 2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y;
            res_coords[j].z = float.Parse(words[i + 3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z;
        }

        /*
        foreach (var coord in residueCoords)
        {
            Debug.Log("ResidueCoord: " + $"<{coord}>");
        }
        */
    }

    // Initializes residues position.
    void initializeResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            res_structure[i] = Instantiate(residue, res_coords[i], Quaternion.identity, residues);
            res_structure[i].name = "Residue" + i.ToString();
        }

        /*
        for (var i = 0; i < n_mol; i++)
        {
            Debug.Log("ResidueCoord[" + i + "]: " + mol_coords[i].ToString("F8"));
        }
        */
    }

    // Initializes bonds position.
    void initializeBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {
            // Define the mean position between a residue and its neighbour to place the bond coordinates.
            bond_coords[i] = (res_coords[i] + res_coords[i + 1]) / 2;
            bond_rotations[i] = Quaternion.FromToRotation(Vector3.down, res_coords[i + 1] - res_coords[i]);

            bond_structure[i] = Instantiate(bond, bond_coords[i], bond_rotations[i], bonds);
            bond_structure[i].name = "Bond" + i.ToString();
        }

        /*
        for (var i = 0; i < n_mol - 1; i++)
        {
            Debug.Log("BondCoord[" + i + "]: " + $"<{bond_coords[i]}>");
        }
        */
    }
    

    void asignResiduesJoints()
    {
        // The first residue ([0]) is conneced to the world origin coordinate.
        // The other residues are connected to the bond that precede them.
        for(var i = 1; i < n_mol; i++)
        {
            res_structure[i].GetComponent<FixedJoint>().connectedBody = bond_structure[i - 1].GetComponent<Rigidbody>();
        }   
    }

    void asignBondsJoints()
    {
        // The first bond is connected to the first residue.
        bond_structure[0].GetComponent<ConfigurableJoint>().connectedBody = res_structure[0].GetComponent<Rigidbody>();

        // The other bonds are connected to the bond that precede them.
        for (var i = 1; i < n_mol - 1; i++)
        {
            bond_structure[i].GetComponent<ConfigurableJoint>().connectedBody = bond_structure[i - 1].GetComponent<Rigidbody>();
        }
    }

    void asignResidueColor()
    {
        for (var i = 0; i < n_mol; i++)
        {
            if (sequence[i] == 'A')
            {
                // Initialize the hydrophobic residue color
                res_structure[i].GetComponent<Renderer>().material.color = Color.red; 
            }
            else
            {
                // Initialize the polar residue color
                res_structure[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    void markFirstResidue()
    {
        first_mol = Instantiate(first, res_coords[0], Quaternion.identity, first_ref);
        first_mol.name = "First0";
    }
}
