using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the saved data storage object.
/// Serialization using JSON format.
/// </summary>
[System.Serializable]
public class SaveData
{
    public string file_name;
    public string origin_name;
    public int n_mol;
    public string sequence;
    public float best_energy;
    public float score;
    public Vector3[] residues_coords;
    public Quaternion[] residues_rotations;
    public Vector3[] bonds_coords;
    public Quaternion[] bonds_rotations;
    public Vector3 camera_position;
    public Quaternion camera_rotation;
}
