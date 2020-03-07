using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Class to implements the verification if a save file is already set (whether the save_file name is null or not).
// IF null: Goes straight to the input/keyboard panel to get a save name.
// ELSE: Offers a choice to the user, in the Game Save Panel.
public class SaveClick : MonoBehaviour
{

    [SerializeField]
    private GameObject save_panel;

    [SerializeField]
    private GameObject input_panel;

    [SerializeField]
    private GameObject keyboard_container;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void checkSaveFileOnClick()
    {
        string save_file = PlayerPrefs.GetString("Save_File");

        if (string.IsNullOrEmpty(save_file))
        {
            noFileRoutine();
        }
        else
        {
            saveChoiceRoutine();
        }
    }

    private void noFileRoutine()
    {
        input_panel.SetActive(true);
        input_panel.GetComponent<InputPanelSetText>().setInputText("Entry the file name:");
        keyboard_container.SetActive(true);
    }

    private void saveChoiceRoutine()
    {
        save_panel.SetActive(true);
    }
}
