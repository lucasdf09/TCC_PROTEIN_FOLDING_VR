using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ignores the collision between the bond and the neighbour residue - which is kind of attached to it.
/// </summary>
public class BondCollisionHandler : MonoBehaviour
{
    private static GameObject[] residues;       // Residues Array reference.
    private static GameObject[] bonds;          // Bonds Array reference.
    private int index;                          // Object bond array index.

    void Start()
    {
        residues = StructureInitialization.residues_structure;
        bonds = StructureInitialization.bonds_structure;
        // Get the index of the current object in the Bonds array.
        index = System.Array.IndexOf(bonds, gameObject);
        // Ignore the collision with the neighbour residue, with the same index in the Residues array. 
        Physics.IgnoreCollision(residues[index].GetComponent<Collider>(), GetComponent<Collider>());  
    }
}
