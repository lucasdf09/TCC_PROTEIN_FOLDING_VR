using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Globalization;


/// <summary>
/// Structure (protein) attributes and methods.
/// </summary>
public class StructureInitialization : MonoBehaviour
{   
    public GameObject residue;                      // Residue Prefab refernce
    public GameObject bond;                         // Bond Prefab reference
    public GameObject first;                        // First Prefab reference
    public Transform residues;                      // Instatitated Parent reference
    public Transform bonds;                         // Instatitated Parent reference
    public Transform first_ref;                     // Instatitated Parent reference

    public static int n_mol;                        // Residue number 
    public static GameObject[] residues_structure;  // Array to store residues objects
    public static GameObject[] bonds_structure;     // Array to store bonds objects
    public static GameObject first_mol;             // Object to store first object
    public static Vector3[] residues_coords;        // Residues coordinates
    public static Quaternion[] residues_rotations;  // Residues rotations
    public static Vector3[] bonds_coords;           // Bonds coordinates
    public static Quaternion[] bonds_rotations;     // Bonds rotations
    public static Vector3 pos_offset;               // Original structure off-set (x, y, z)
    public static string sequence;                  // AB sequence
    public static string origin_name;               // The name of the protein simulation input file

    private GameFilesHandler files_handler;         // Game Files Handler reference.


    // Start is called before the first frame update.
    // Runs before PlayerController initializeGame()
    private void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();

        // New Game Structure initialization
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.New_game)))
        {
            string file_name = PlayerPrefs.GetString(GameFilesHandler.New_game);

            // Get the name of the original protein file and set a reference name 
            origin_name = Path.GetFileNameWithoutExtension(file_name);

            // Read data in a Txt file to a string
            string read_data = files_handler.readTxtFile(file_name);
            // Check the string generated and loads only the necessary data to build a structure
            if (!string.IsNullOrEmpty(read_data))
            {
                loadInput(read_data);
                buildStructure();
            }                      
        }
        // Saved Game Structure initialization
        else if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.Saved_game)))
        {
            string file_name = PlayerPrefs.GetString(GameFilesHandler.Saved_game);
            files_handler.loadGame(file_name);
            loadStructure();
        }
        else
        {
            Debug.Log("StructureInitialization Error: No File Loaded!");
        }
    }

    /// <summary>
    /// Load the input file data to the structure attributes.
    /// </summary>
    /// <param name="read_data">String with a structure data formated</param>
    private void loadInput(string read_data)
    { 
        Debug.Log("StructureInitialization.loadInput()");
        // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array
        string[] words = read_data.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //Get the molecules data
        n_mol = int.Parse(words[Array.IndexOf(words, "Molecules") + 2]);
        Debug.Log("n_mol (Molecules) : " + n_mol);

        // Get the sequence data
        sequence = words[Array.IndexOf(words, "Sequence") + 2];
        Debug.Log("Sequence: " + sequence);

        // Takes the offset from the center (0.0 x , 0.0 y , 0.0 z) of the simulation box
        pos_offset.x = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.y = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.z = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
        Debug.Log("Position offset: " + pos_offset);

        // Initialize the structure in the center of the Scene with the first residue in Vector3 (0, 0, 0)
        residues_coords = new Vector3[n_mol];
        for (var i = 0; i < n_mol * 4; i += 4)
        {
            var j = i / 4;
            residues_coords[j].x = float.Parse(words[i + 1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x;
            residues_coords[j].y = float.Parse(words[i + 2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y;
            residues_coords[j].z = float.Parse(words[i + 3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z;
        }
    }


    /// <summary>
    /// Builds a structure for a new game.
    /// </summary>
    public void buildStructure()
    {
        residues_structure = new GameObject[n_mol];
        bonds_structure = new GameObject[n_mol - 1];
        bonds_coords = new Vector3[n_mol - 1];
        bonds_rotations = new Quaternion[n_mol - 1];

        initializeResidues();       
        initializeBonds();

        lockStructure();

        //assignResiduesJoints();
        assignBondsJoints();
        assignBondFixedJoints();

        setResiduesColor();
        markFirstResidue();
    }

    /// <summary>
    /// Destroys a structure objects. 
    /// Not in Use.
    /// </summary>
    public void destroyStructure()
    {
        // Destroy the residues and bonds objects
        for (var i = 0; i < n_mol - 1; i++)
        {
            Destroy(residues_structure[i]);
            Destroy(bonds_structure[i]);
        }
        // Destroy the last residue
        Destroy(residues_structure[n_mol - 1]);
        // Destroy the First object
        Destroy(first_mol);
    }

    /// <summary>
    /// Builds a strucuture for a saved game.
    /// </summary>
    public void loadStructure()
    {
        residues_structure = new GameObject[n_mol];
        bonds_structure = new GameObject[n_mol - 1];
        loadResidues();
        loadBonds();

        lockStructure();

        //assignResiduesJoints();
        assignBondsJoints();
        assignBondFixedJoints();

        setResiduesColor();
        markFirstResidue();
    }

    /// <summary>
    /// Initializes residues for a new game.
    /// </summary>
    private void initializeResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            residues_structure[i] = Instantiate(residue, residues_coords[i], Quaternion.identity, residues);
            residues_structure[i].name = "Residue" + i.ToString();
        }
    }

    /// <summary>
    /// Initializes residues for saved game.
    /// </summary>
    private void loadResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            residues_structure[i] = Instantiate(residue, residues_coords[i], residues_rotations[i], residues);
            residues_structure[i].name = "Residue" + i.ToString();
        }
    }

    /// <summary>
    /// Initializes bonds for a new game.
    /// </summary>
    private void initializeBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {
            // Define the mean position between a residue and its neighbour to place the bond coordinates.
            bonds_coords[i] = (residues_coords[i] + residues_coords[i + 1]) / 2;
            // Define the bond rotation based on the neighbour residues.
            bonds_rotations[i] = Quaternion.FromToRotation(Vector3.down, residues_coords[i + 1] - residues_coords[i]);
            bonds_structure[i] = Instantiate(bond, bonds_coords[i], bonds_rotations[i], bonds);
            bonds_structure[i].name = "Bond" + i.ToString();
        }      
    }

    /// <summary>
    /// Initializes bonds for a saved game.
    /// </summary>
    private void loadBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {          
            bonds_structure[i] = Instantiate(bond, bonds_coords[i], bonds_rotations[i], bonds);
            bonds_structure[i].name = "Bond" + i.ToString();
        }
    }

    /// <summary>
    /// Locks the residues and bonds setting the Rigidbody components as kinematic. 
    /// </summary>
    private void lockStructure()
    {
        foreach (var residue in residues_structure)
            residue.GetComponent<Rigidbody>().isKinematic = true;

        foreach (var bond in bonds_structure)
            bond.GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// Assigns the residues joints.
    /// </summary>
    private void asignResiduesJoints()
    //public void assignResiduesJoints()
    {
        // The first residue ([0]) is conneced to the world origin coordinate - by default
        // The other residues are connected to the bond that precede them
        //for (var i = 1; i < n_mol; i++)
        for (var i = n_mol - 1; i > 0; i--)
        {
            residues_structure[i].GetComponent<FixedJoint>().connectedBody = bonds_structure[i - 1].GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Assigns the bonds configurable joints. 
    /// Connects a bond joint with the previous bond.
    /// Except for the first bond.
    /// </summary>
    private void assignBondsJoints()
    {
        // The first bond is connected to the first residue
        bonds_structure[0].GetComponent<ConfigurableJoint>().connectedBody = residues_structure[0].GetComponent<Rigidbody>();
        // The other bonds are connected to the bond that precede them
        for (var i = 1; i < n_mol - 1; i++)
        {
            bonds_structure[i].GetComponent<ConfigurableJoint>().connectedBody = bonds_structure[i - 1].GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Assigns the bonds fixed joints.
    /// Connects a bonds joint with the next residue. 
    /// </summary>
    private void assignBondFixedJoints()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {
            bonds_structure[i].GetComponent<FixedJoint>().connectedBody = residues_structure[i + 1].GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Sets the residues color.
    /// </summary>
    private void setResiduesColor()
    {
        for (var i = 0; i < n_mol; i++)
        {
            if (sequence[i] == 'A')
            {
                // Initialize the hydrophobic residues color as red
                residues_structure[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                // Initialize the polar residues color as blue
                residues_structure[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    /// <summary>
    /// Instatiates the First object and associates it with the first residue.
    /// </summary>
    private void markFirstResidue()
    {
        //first_mol = Instantiate(first, residues_structure[0].transform.position, Quaternion.identity, first_ref);
        first_mol = Instantiate(first, residues_structure[0].transform.position, Quaternion.identity, residues_structure[0].transform);
        first_mol.name = "First0";
        //residues_structure[0].transform.SetParent(first_mol.transform);
        //first_mol.GetComponent<FixedJoint>().connectedBody = residues_structure[0].GetComponent<Rigidbody>();
        //residues_structure[0].GetComponent<Rigidbody>().isKinematic = true;
    }

//  End of StructureInitialization
}
