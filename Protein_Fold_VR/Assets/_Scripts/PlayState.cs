using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to store the structure state during the gameplay.
/// Used in the undo/redo operations.
/// </summary>
public class PlayState
{
    public Vector3[] residues_position;        
    public Vector3[] bonds_position;
    public Quaternion[] residues_rotation;
    public Quaternion[] bonds_rotation;

    /// <summary>
    /// Constructor that initializes the class attributes.
    /// </summary>
    /// <param name="new_residues_position">State residues position values.</param>
    /// <param name="new_bonds_position">State bonds position values.</param>
    /// <param name="new_residues_rotation">State residues rotation values.</param>
    /// <param name="new_bonds_rotation">State bonds rotation values.</param>
    public PlayState (Vector3[] new_residues_position, Vector3[] new_bonds_position, Quaternion[] new_residues_rotation, Quaternion[] new_bonds_rotation)
    {
        residues_position = new_residues_position;
        bonds_position = new_bonds_position;
        residues_rotation = new_residues_rotation;
        bonds_rotation = new_bonds_rotation;
    }
}
