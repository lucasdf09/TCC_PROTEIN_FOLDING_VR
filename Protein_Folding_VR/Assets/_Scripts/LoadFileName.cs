using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFileName : MonoBehaviour
{
    public void LoadName(string fileName)
    {
        PlayerPrefs.SetString("File_Name", fileName);
    }
}
