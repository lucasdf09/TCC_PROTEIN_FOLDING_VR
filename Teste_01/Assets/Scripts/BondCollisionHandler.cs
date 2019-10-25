using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondCollisionHandler : MonoBehaviour
{
    private static GameObject[] residues;       // Residues Array reference.
    private static GameObject[] bonds;          // Bonds Array reference.
    private int index;                          // object bond array index.

    void Start()
    {
        residues = StructureInitialization.mol_structure;
        bonds = StructureInitialization.bond_structure;
        // Get the index of the current object in the Bonds array.
        index = System.Array.IndexOf(bonds, gameObject);
        // Ignore the collision with the neighbour residue, with the same index in the Residues array. 
        Physics.IgnoreCollision(residues[index].GetComponent<Collider>(), GetComponent<Collider>());  
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " Enter: " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(gameObject.name + " Stay: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(gameObject.name + " Exit: " + other.name);
    }
}
