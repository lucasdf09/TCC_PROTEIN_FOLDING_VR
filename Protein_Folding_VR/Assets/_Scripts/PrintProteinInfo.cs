using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the generation and printing of structure information.
/// </summary>
public class PrintProteinInfo : MonoBehaviour
{
    [SerializeField]
    private Text info_text = default;     // Reference to protein info panel text viewer

    /// <summary>
    /// Generates a string with the structure informations and prints it in the Protein Info Panel. 
    /// </summary>
    public void printInfo()
    {
        string info = "";
        info = "Name: " + StructureInitialization.origin_name + "\n";
        info += "Molecules: " + StructureInitialization.n_mol + "\n";
        info += "Sequence: " + StructureInitialization.sequence + "\n";
        info += "Score: " + PlayerController.score.ToString("F3") + "\n";
        info += "Best Energy: " + PlayerController.best_energy.ToString("F3") + "\n";
        info += "Potential Energy: " + PlayerController.potential_energy.ToString("F3") + "\n";
        info += "Bond Energy: " + PlayerController.bond_energy.ToString("F3") + "\n";
        info += "Torsion Energy: " + PlayerController.torsion_energy.ToString("F3") + "\n";
        info += "Lennard-Jones Energy: " + PlayerController.lj_energy.ToString("F3") + "\n";
        info += "Radius of Gyration (Rg): " + PlayerController.rg_all.ToString("F3") + "\n";
        info += "Hydrophobic Rg: " + PlayerController.rg_h.ToString("F3") + "\n";
        info += "Polar Rg: " + PlayerController.rg_p.ToString("F3") + "\n";

        info_text.text = info;

        Debug.Log("PrintInfo:/n" + info);
        /*
        // New Game Structure Info
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.New_game)))
        {
            string info = "";
            info = "Name = " + StructureInitialization.origin_name + "\n";
            info += "Molecules = " + StructureInitialization.n_mol + "\n";
            info += "Sequence = " + StructureInitialization.sequence + "\n";
            info += "Score = " + PlayerController.score.ToString("F3") + "\n";
            info += "Best Energy = " + PlayerController.best_energy.ToString("F3") + "\n";
            info += "Potential Energy = " + PlayerController.potential_energy.ToString("F3") + "\n";
            info += "Bond Energy = " + PlayerController.bond_energy.ToString("F3") + "\n";
            info += "Torsion Energy = " + PlayerController.torsion_energy.ToString("F3") + "\n";
            info += "Lennard-Jones Energy = " + PlayerController.lj_energy.ToString("F3") + "\n";
            info += "Radius of Gyration (Rg) = " + PlayerController.rg_all.ToString("F3") + "\n";
            info += "Hydrophobic Rg = " + PlayerController.rg_h.ToString("F3") + "\n";
            info += "Polar Rg = " + PlayerController.rg_p.ToString("F3") + "\n";
        }
        // Saved Game Structure Info
        else if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.Saved_game)))
        {

        }
        else
        {
            Debug.Log("PrintProteinInfo: Couldn't find strucure reference file!");
        }
        */
    }



}
