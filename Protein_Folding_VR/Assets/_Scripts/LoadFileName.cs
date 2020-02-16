using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFileName : MonoBehaviour
{
    public void LoadName()
    {
        //PlayerPrefs.SetString("File_Name", fileName);
        PlayerPrefs.SetString("File_Name", gameObject.GetComponent<GameListButton>().Button_file);
        Debug.Log("Loaded: " + PlayerPrefs.GetString("File_Name"));
    }
}
