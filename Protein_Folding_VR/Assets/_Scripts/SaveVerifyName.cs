using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveVerifyName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void verifySaveName(string file_name)
    {
        // Empty name
        if (string.IsNullOrEmpty(file_name))
        {
            Debug.Log("Verify: Null or Empty");
        }
        // All white space name
        else if (string.IsNullOrWhiteSpace(file_name))
        {
            Debug.Log("Verify: Null or WhiteSpace");
        }
        // Name begins with white space
        else if (file_name[0].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in First");
        }
        // Name ends with white space
        else if (file_name[file_name.Length - 1].Equals(' '))
        {
            Debug.Log("Verify: WhiteSpace in Last");
        }
        // Name already exists
        else if (saveFileExists(file_name))
        {
            Debug.Log("Verify: Name already exists");
        }
        // Valid name
        else
        {

        }

        
    }

    private bool saveFileExists(string file_name)
    {

        return true;
    }
}
