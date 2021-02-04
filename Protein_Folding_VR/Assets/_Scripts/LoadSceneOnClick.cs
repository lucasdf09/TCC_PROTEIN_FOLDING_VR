using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load a new Scene.
/// </summary>
public class LoadSceneOnClick : MonoBehaviour
{
    /// <summary>
    /// Load the scene index associated scene.
    /// </summary>
    /// <param name="scene_index">The scene Index in File > Build Settings.</param>
    public void LoadByIndex(int scene_index)
    {
        SceneManager.LoadScene(scene_index);
    }
}
