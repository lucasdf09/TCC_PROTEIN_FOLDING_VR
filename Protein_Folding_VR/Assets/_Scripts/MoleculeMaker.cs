using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeMaker : MonoBehaviour
{
    public static int n_mol;
    public static string sequence;

    public GameObject structure;                // Structure Prefab reference.
    public GameObject residue;                 // Residue Prefab refernce.
    public GameObject bond;                     // Bond Prefab reference.
    public GameObject first;                    // First Prefab reference.

    public Transform molecule;                  // Instantiated Prefab reference.
    public static Vector3 pos_offset;

    public static GameObject[] res_structure;
    public static GameObject[] bond_structure;
    public static GameObject first_mol;

    // Start is called before the first frame update
    void Start()
    {
        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;

        res_structure = new GameObject[n_mol];
        bond_structure = new GameObject[n_mol - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            initializeResidues();
            initializeBonds();
            asignResiduesJoints();
            asignBondsJoints();
            asignResidueColor();
            markFirstResidue();
            Debug.Log("Molecule Maker activated!");
        }
    }

    // Initializes residues position.
    void initializeResidues()
    {
        Transform residues = molecule.transform.Find("Residues").transform;
        GameObject[] res_structure0 = StructureInitialization.res_structure;
        
        for (var i = 0; i < n_mol; i++)
        {
            res_structure[i] = Instantiate(residue, res_structure0[i].transform.position, Quaternion.identity, residues);
            res_structure[i].name = "Residue" + i.ToString();
        }
    }

    void initializeBonds()
    {
        Transform bonds = molecule.transform.Find("Bonds").transform;
        GameObject[] bond_structure0 = StructureInitialization.bond_structure;

        for (var i = 0; i < n_mol - 1; i++)
        {
            // Define the mean position between a residue and its neighbour to place the bond coordinates.
            //bond_coords[i] = (res_coords[i] + res_coords[i + 1]) / 2;
            //bond_rotations[i] = Quaternion.FromToRotation(Vector3.down, res_coords[i + 1] - res_coords[i]);

            bond_structure[i] = Instantiate(bond, bond_structure0[i].transform.position, bond_structure0[i].transform.rotation, bonds);
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
        for (var i = 1; i < n_mol; i++)
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
            if (sequence[i] == 'A' || sequence[i] == 'a')
            {
                // Initialize the hydrophobic residue color
                res_structure[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else if (sequence[i] == 'B' || sequence[i] == 'b')
            {
                // Initialize the polar residue color
                res_structure[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    void markFirstResidue()
    {
        first_mol = Instantiate(first, res_structure[0].transform.position, Quaternion.identity, molecule.transform.Find("First").transform);
        first_mol.name = "First0";
    }
}
