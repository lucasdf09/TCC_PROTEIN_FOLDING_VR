using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the settings data storage object.
/// Serialization using JSON format.
/// </summary>
[System.Serializable]
public class SettingsData
{
    public Vector3 score_position;
    public Quaternion score_rotation;
    public bool score_toogle;
    public Vector3 parameters_postiton;
    public Quaternion parameters_rotation;
    public bool parameters_toogle;
}
