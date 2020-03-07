using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestLoadPref : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("Save_File",  null);
    }
}
