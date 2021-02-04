using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the application finishing. 
/// </summary>
public class ExitOnClick : MonoBehaviour
{
    /// <summary>
    /// Closes the application.
    /// </summary>
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
