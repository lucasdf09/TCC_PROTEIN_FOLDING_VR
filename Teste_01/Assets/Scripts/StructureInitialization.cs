using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System;
using System.Globalization;

public class StructureInitialization : MonoBehaviour
{
    public static int n_mol;
    public GameObject molecule;                 // Residue Prefab refernce.
    public GameObject bond;                     // Bond Prefab reference.
    public GameObject first;                    // First Prefab reference.
    public Transform molecules;                 // Instatitated Prefab reference.
    public Transform bonds;                     // Instatitated Prefab reference.
    public Transform first_ref;                 // Instatitated Prefab reference.
    public static GameObject[] mol_structure;
    public static GameObject[] bond_structure;
    public static GameObject first_mol;
    public static Vector3[] mol_coords;
    public static Vector3[] bond_coords;
    public static Quaternion[] bond_rotations;
    public static Vector3 pos_offset;
    public static string sequence;
    //public static bool select_mode;
    //public static bool move_mode;
    //public static GameObject first;
    //public static float scale;

    // Start is called before the first frame update.
    void Start()
    {
        readInput();
        mol_structure = new GameObject[n_mol];
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
    void readInput()
    {

        string path = "Assets/Resources/Inputs/13_teste_bestCoordinates.txt";

        // 1HJM input file.
        //string path = "Assets/Resources/Inputs/104_1HJM.txt";

        // Loads all the TXT input file to "words[]".
        StreamReader reader = new StreamReader(path);
        string full_text = reader.ReadToEnd();
        
        // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array.
        string[] words = full_text.Split(new char[] { ' ', '\t', '\r', '\n' } , StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < words.Length; i++)
        {
        //    Debug.Log("words[" + i + "] = " + words[i]);
        }

        n_mol = int.Parse(words[Array.IndexOf(words, "Molecules") + 2]);
        Debug.Log("n_mol (Molecules) : " + n_mol);

        sequence = words[Array.IndexOf(words, "Sequence") + 2];
        Debug.Log("Sequence: " + sequence);


        pos_offset.x = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.y = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
        pos_offset.z = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);

        Debug.Log("Position offset: " + $"<{pos_offset}>");

        mol_coords = new Vector3[n_mol];

        for (var i = 0; i < n_mol * 4; i += 4)
        {
            var j = i / 4;
            //residueCoords[j].x = (float.Parse(words[i+1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x) * scale;
            //residueCoords[j].y = (float.Parse(words[i+2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y) * scale;
            //residueCoords[j].z = (float.Parse(words[i+3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z) * scale;
            mol_coords[j].x = float.Parse(words[i + 1], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.x;
            mol_coords[j].y = float.Parse(words[i + 2], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.y;
            mol_coords[j].z = float.Parse(words[i + 3], CultureInfo.InvariantCulture.NumberFormat) - pos_offset.z;
        }

        /*
        foreach (var coord in residueCoords)
        {
            Debug.Log("ResidueCoord: " + $"<{coord}>");
        }
        */

        reader.Close();
    }

    // Initializes residues position.
    void initializeResidues()
    {
        for (var i = 0; i < n_mol; i++)
        {
            mol_structure[i] = Instantiate(molecule, mol_coords[i], Quaternion.identity, molecules);
            mol_structure[i].name = "Residue" + i.ToString();
        }

        for (var i = 0; i < n_mol; i++)
        {
            Debug.Log("ResidueCoord[" + i + "]: " + mol_coords[i].ToString("F8"));
        }
    }

    // Initializes bonds position.
    void initializeBonds()
    {
        for (var i = 0; i < n_mol - 1; i++)
        {
            // Define the mean position between a residue and its neighbour to place the bond coordinates.
            bond_coords[i] = (mol_coords[i] + mol_coords[i + 1]) / 2;
            bond_rotations[i] = Quaternion.FromToRotation(Vector3.down, mol_coords[i + 1] - mol_coords[i]);

            bond_structure[i] = Instantiate(bond, bond_coords[i], bond_rotations[i], bonds);
            bond_structure[i].name = "Bond" + i.ToString();
        }

        for (var i = 0; i < n_mol - 1; i++)
        {
        //    Debug.Log("BondCoord[" + i + "]: " + $"<{bond_coords[i]}>");
        }
    }
    

    void asignResiduesJoints()
    {
        // The first residue ([0]) is conneced to the world origin coordinate.
        // The other residues are connected to the bond that precede them.
        for(var i = 1; i < n_mol; i++)
        {
            mol_structure[i].GetComponent<FixedJoint>().connectedBody = bond_structure[i - 1].GetComponent<Rigidbody>();
        }   
    }

    void asignBondsJoints()
    {
        // The first bond is connected to the first residue.
        bond_structure[0].GetComponent<ConfigurableJoint>().connectedBody = mol_structure[0].GetComponent<Rigidbody>();

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
                mol_structure[i].GetComponent<Renderer>().material.color = Color.red; 
            }
            else
            {
                // Initialize the polar residue color
                mol_structure[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    void markFirstResidue()
    {
        first_mol = Instantiate(first, mol_coords[0], Quaternion.identity, first_ref);
    }
}
