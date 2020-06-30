using System;
using System.Collections;
using System.Collections.Generic;
//using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Input file verification routine to begin a new game.
/// </summary>
public class VerifyInputFile : MonoBehaviour
{
    private GameFilesHandler files_handler;             // Reference to GameFilesHandler
    private string message;                             // Invalid file message

    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void verifyInputFile()
    {
        Debug.Log("Verify Input Files:");
        string file_path = PlayerPrefs.GetString(GameFilesHandler.New_game);
        Debug.Log("File path: " + file_path);
        if (validFile(file_path))
        {
            gameObject.GetComponent<ConfirmNewGame>().confirmGame();
        }
        else
        {
            gameObject.GetComponent<NotifyNewGameInvalid>().notifyInvalid("Can't start a new game!\n" + Path.GetFileNameWithoutExtension(PlayerPrefs.GetString(GameFilesHandler.New_game)) + " " + message);
        }
    }


    /// <summary>
    /// Tests a file if it's in the proper format for a New Game.
    /// </summary>
    /// <param name="file_path">Input file path.</param>
    /// <returns>True if the file is valid.</returns>
    private bool validFile(string file_path)
    {       
        // Read data in a Txt file to a string
        string read_data = files_handler.readTxtFile(file_path);
        // Declare and initialize the validation flag
        bool isValid = false;
        // Check if the file is empty
        if (!string.IsNullOrEmpty(read_data))
        {
            // Split the words in the input between the ' ', '\t', '\r', '\n' characters, removing them from the resulting Array
            string[] words = read_data.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int n_mol;
            string sequence;
            // Check if tag "Molecules" exist
            if (words.Contains("Molecules"))
            {
                // Check if the Molucules index don't exceed the file limit                
                if ((Array.IndexOf(words, "Molecules") + 2) < words.Length)
                {
                    // Check if Molecules has an integer associated
                    if (int.TryParse(words[Array.IndexOf(words, "Molecules") + 2], out n_mol))
                    {
                        // Check if the integer is positive
                        if (n_mol > 0)
                        {
                            // Check if tag "Sequence" exists
                            if (words.Contains("Sequence"))
                            {
                                // Check if Sequence has a string associated and don't exceed the file limit                                
                                if ((Array.IndexOf(words, "Sequence") + 2) < words.Length)
                                {
                                    sequence = words[Array.IndexOf(words, "Sequence") + 2];                               
                                    // Check if the sequence has the same size as molecules
                                    if (sequence.Length == n_mol)
                                    {
                                        // Check if the string has only As and Bs characters
                                        if (sequence.All(character => character == 'A' || character == 'B'))
                                        {
                                            Vector3[] coordinates = new Vector3[n_mol];
                                            var style = System.Globalization.NumberStyles.Number;
                                            var culture = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
                                            // Check if the residues coordinates are in a valid format
                                            for (var i = 0; i < n_mol; i++)
                                            {
                                                string identificator = "coordinate " + i + " : ";
                                                // Check if the residue identificator is an integer
                                                if (int.TryParse(words[i * 4], out int residue))
                                                {
                                                    // Check if the identificator is in ascending order
                                                    if (residue == i)
                                                    {
                                                        // Check if the x coordinate is a float
                                                        if (float.TryParse(words[i * 4 + 1], style, culture, out float x))
                                                        {
                                                            // Check if the y coordinate is a float
                                                            if (float.TryParse(words[i * 4 + 2], style, culture, out float y))
                                                            {
                                                                // Check if the z coordinate is a float
                                                                if (float.TryParse(words[i * 4 + 3], style, culture, out float z))
                                                                {
                                                                    // Don't need to verify the first coordinate
                                                                    if (i == 0)
                                                                    {
                                                                        coordinates[i] = new Vector3(x, y, z);
                                                                        isValid = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        Vector3[] sub_coordinates = new Vector3[i];
                                                                        Array.Copy(coordinates, sub_coordinates, i);
                                                                        // Check if the coordinate already exists
                                                                        // in the sub array of the values that have already been read
                                                                        if (!sub_coordinates.Contains(new Vector3(x, y, z)))
                                                                        {
                                                                            coordinates[i] = new Vector3(x, y, z);
                                                                            var distance = Vector3.Distance(coordinates[i], coordinates[i - 1]);
                                                                            // Check if the cordinate has a distance from the neighbour between 0.9 and 1.1
                                                                            if ((distance > 0.9) && (distance < 1.1))
                                                                            {
                                                                                isValid = true;
                                                                            }
                                                                            else
                                                                                message = identificator + "invalid neighbour distance.";
                                                                        }
                                                                        else
                                                                            message = identificator + "values already exists in other residue.";
                                                                    }
                                                                }
                                                                else
                                                                    message = identificator + "Z value couldn't be parsed to a float.";
                                                            }
                                                            else
                                                                message = identificator + "Y value couldn't be parsed to a float.";
                                                        }
                                                        else
                                                            message = identificator + "X value couldn't be parsed to a float.";
                                                    }
                                                    else
                                                        message = identificator + "identificator is in the wrong order.";
                                                }
                                                else
                                                    message = identificator + "identificator isn't an integer.";

                                                // If a coordinate is not valid, the coordinates verification (FOR loop) is stopped
                                                if (!isValid)
                                                {
                                                    Debug.Log("BREAK in coordinate " + i);
                                                    break;
                                                }
                                                // Reset the flag to test the next coordinate until the last (n_mol-1)
                                                if (i < n_mol - 1)
                                                {
                                                    isValid = false;
                                                }
                                            }
                                        }
                                        else
                                            message = "invalid Sequence associated string.This must contain only A or B characters.";
                                    }
                                    else
                                        message = "Sequence string length is different from Molecules value.";
                                }
                                else
                                    message = "Sequence string is null or empty.";
                            }
                            else
                                message = "Sequence tag not found.";
                        }
                        else
                            message = "Molecules value is negative.";
                    }
                    else
                        message = "invalid Molecules associated string. This must be an integer.";
                }
                else
                    message = "invalid Molecules associated string. Value is null.";
            }
            else
                message = "Molecules tag not found.";               
        }
        else
            message = "file is null or empty.";

        return isValid;
    }

}
