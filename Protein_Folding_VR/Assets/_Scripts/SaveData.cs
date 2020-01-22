using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that implements the save data storage object.
// Serialization using JSON format.
//[System.Serializable]
public class SaveData
{
    public int n_mol;
    public string sequence;
    public float best_energy;
    public float score;
    public Vector3[] residues_coords;
    public Quaternion[] residues_rotations;
    public Vector3[] bonds_coords;
    public Quaternion[] bonds_rotations;
}
