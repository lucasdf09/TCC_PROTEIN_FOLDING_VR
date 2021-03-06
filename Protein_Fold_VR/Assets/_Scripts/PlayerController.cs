﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class to implement the principal user interaction functionalities.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public GameObject player_canvas;        // Object reference to Score and Parameters
    public Text score_text;                 // Score text reference
    public Text parameters_text;            // Parameters text reference
    public GameObject reticle_pointer;      // Reticle Pointer reference
    public GameObject camera_pivot;         // Camera Pivot reference
    public float rotation_angle;            // Rotation angle rate (to move around the structure)
    public float zoom_smooth;               // Zoom speed rate
    public float pivot_smooth;              // Pivot change position speed rate
    public GameObject menu_container;       // Menu container object reference
    public GameObject keyboard_container;   // Keyboard container object reference
    public GameObject structure;            // Structure object reference
    public float delta;                     // Residue translation rate
    public float blink_duration;            // Blink period increasing rate
    public GameObject toggle_score;         // Reference to Score toggle
    public GameObject toggle_parameters;    // Reference to Parameters toggle

    public static bool select_mode;         // Flag to signal the select residue mode
    public static bool move_mode;           // Flag to signal the residue movement mode
    public static GameObject target;        // Gazed/Selected residue reference
    public static Color target_color;       // Target residue color reference
    public static float score;              // Diference between the initial best energy and the actual
    public static float saved_score;        // Score loaded from a saved file to set a new best energy start parameter
    public static float best_energy;        // Best potential energy reference for score calculation
    public static float potential_energy;   // 3D AB off-lattice model Potential Energy 
    public static float bond_energy;        // 3D AB off-lattice model Bond Energy
    public static float torsion_energy;     // 3D AB off-lattice model Torsion Energy
    public static float lj_energy;          // 3D AB off-lattice model Lennard-Jones Energy
    public static float rg_all;             // All residues Radius of Gyration (R.G.)
    public static float rg_h;               // Hydrophobic residues R.G. 
    public static float rg_p;               // Polar residues R.G.
    public static Vector3 camera_position;          // Camera position reference for saving
    public static Quaternion camera_rotation;       // Camera rotation reference for saving
    public static GameObject score_display;         // Score display position reference
    public static GameObject parameters_display;    // Parameters display position reference
    public static GameObject score_toggle_button;          // Score toggle button reference
    public static GameObject parameters_toggle_button;     // Parameters toggle button reference

    private static Color color_end;             // Color to blink with the original
    private static Color orange_color;          // Orange color;
    private GameObject[] particles;             // Reference to residues
    private GameObject[] bonds;                 // Reference to bonds
    private int n_mol;                          // Number of molecules
    private Color color_aux;                    // Auxiliar color object to store the residue own color during blink        
    private string sequence;                    // AB Protein sequence 
    private Vector3 center_mass;                // Residues Center of Mass cartesian coordinates
    private GameFilesHandler files_handler;     // Game Files Handler reference

    private List<PlayState> undo_list;          // List that contains the structure states
    private int undo_index;                     // Counter to the array position
    private int list_size;                      // Movements record list size
    private bool moved_flag;                    // Flag to signal movement
    private bool undo_flag;                     // Flag to signal undo operation
    private bool updating_score;                // Flag to signal the updating structure coroutine calls


    // DEBUG
    /*
    int calc_distance = 3;
    int var_residue = 3;
    int var_bond = 0;
    Vector3[] res_variation;                    // Variation of the residues
    Vector3[] bond_variation;                   // Variation of the bonds
    int counter;
    */

    // Awake is called before Start().
    private void Awake()
    {
        // Initialization of attributes
        color_end = Color.yellow;
        orange_color = new Color(1.0f, 0.64f, 0.0f);
        score = 0.0f;
        potential_energy = 0.0f;

        // Deactivate player interaction until initialization finish
        reticle_pointer.SetActive(false);
        select_mode = false;
        move_mode = false;
        Physics.autoSimulation = false;
    }


    private void Start()
    {   
        // Player attributes initialization
        target = null;

        // Undo attributes initialization
        undo_list = new List<PlayState>();
        undo_index = 0;
        list_size = 1048576;
        moved_flag = false;
        undo_flag = false;
        updating_score = false;

        // Structure references initialization
        particles = StructureInitialization.residues_structure;
        bonds = StructureInitialization.bonds_structure;
        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;

        // Display references initialization
        score_display = score_text.gameObject;
        parameters_display = parameters_text.gameObject;
        score_toggle_button = toggle_score;
        parameters_toggle_button = toggle_parameters;
        
        // Get the reference to the GameFilesHandler game object
        GameObject game_files = GameObject.FindGameObjectWithTag("GameFiles");
        files_handler = game_files.GetComponent<GameFilesHandler>();       
        //  Check if there is a settings file saved
        if (files_handler.settingsFileExists(GameFilesHandler.Display_file))
        {
            Debug.Log("PlayerController: Display file found!");
            // Load settings saved in previous use
            files_handler.loadSettings(GameFilesHandler.Display_file);
        }
        else
        {
            Debug.Log("PlayerController: Display file NOT found!");
        }

        // Call initializeGame function in the next frame. 
        Invoke("initializeGame", 0);
    }


    /// <summary>
    /// Initialize the game and the player status.
    /// </summary>
    private void initializeGame()
    {
        initializeParameters();

        refreshScoreboard();
        
        // Insert the first state of the structure in the moves list
        insertState();
        setInitialPosition();
        
        // Activate the player interaction
        reticle_pointer.SetActive(true);
        select_mode = true;
    }


    /// <summary>
    /// Initializes the structures parameters. 
    /// </summary>
    private void initializeParameters()
    {
        // New Game best_energy initialization
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.New_game)))
        {
            calculatePotentialEnergy();
            best_energy = potential_energy;
        }

        unlockStructure();

        var energy = 0.0f;
        calculatePotentialEnergy();
        //var step = 0;

        // Stabilization routine
        do
        {
            energy = potential_energy;
            Physics.Simulate(Time.fixedDeltaTime);
            calculatePotentialEnergy();
            //Debug.Log("Step: " + ++step);           
        } while (energy != potential_energy);
        //} while (!Mathf.Approximately(energy - potential_energy, Mathf.Epsilon));

        Physics.autoSimulation = true;    

        Debug.Log("initializeParameters() Finished");
        //Debug.Log("Best Energy = " + best_energy.ToString("F12"));        
        //Debug.Log("Potential Energy = " + potential_energy.ToString("F12"));       
    }


    // To process the non-playable interaction
    private void Update()
    {
        // SELECT MODE
        // In this mode the player can:
        // Select a residue (to manipulate) using the gaze input (reticle pointer)
        // Move the camera
        // Zoom in/out
        // Access the Game Menu
        if (select_mode)
        {
            // Check the transition to MOVE_MODE
            // If a residues is being gazed and the action button has being pressed
            if (target != null && Input.GetButtonDown("C"))
            {
                //Debug.Log("SELECT: CLICK key was pressed");
                // Get the residue own color
                color_aux = target_color;
                //Set the auxiliar color transparency to blink
                color_aux.a = 0.1f;
                select_mode = false;
                move_mode = true;
                //Debug.Log("Select mode: " + select_mode);
                //Debug.Log("Move mode: " + move_mode);
                reticle_pointer.SetActive(false);
                refreshScoreboard();              
            }

            // Blink the residue on gaze
            if (target != null)
            {
                blinkResidue(target.GetComponent<Renderer>(), target_color, color_end);
            }

            // Menu calling
            //if (Input.GetKeyDown("m"))
            if (Input.GetButtonDown("D"))
            {
                // Reset residue gazed attributes
                if(target != null)
                {
                    target.GetComponent<Renderer>().material.color = target_color;
                    target = null;
                }
                // Disable score
                player_canvas.SetActive(false);
                // Disable select mode
                select_mode = false;
                // Hide the strucure from player view 
                structure.SetActive(false);
                // Set the Game Menu in front of the player view using the camera as reference
                menu_container.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
                menu_container.transform.rotation = Camera.main.transform.rotation;
                menu_container.SetActive(true);
                // Set the Keyboard in front of the player view using the camera as reference
                keyboard_container.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;                
                keyboard_container.transform.rotation = Camera.main.transform.rotation;
                // Rotate the Keyboard to a better position for the player interaction
                keyboard_container.transform.Rotate(30, 0, 0);

                // Get the camera positioning to restore it when load a game
                camera_position = Camera.main.transform.position;
                camera_rotation = Camera.main.transform.rotation;
                //Debug.Log("Camera position is equal? " + Equals(camera_position, Camera.main.transform.position));
                //Debug.Log("Camera rotation is equal? " + Equals(camera_rotation, Camera.main.transform.rotation));          
            }
        }

        // Check the return condition to SELECT MODE, once in MOVE MODE
        else if (move_mode)
        {
            blinkResidue(target.GetComponent<Renderer>(), target_color, color_aux);
            // If the action button was pressed, return to select mode
            if (Input.GetButtonDown("C"))
            {
                //Debug.Log("MOVE: CLICK key was pressed");
                // Reset the residue own color
                target.GetComponent<Renderer>().material.color = target_color;
                target = null;
                reticle_pointer.SetActive(true);
                // Set the camera pivot after the residues movements
                setCameraPivot();               
                select_mode = true;
                move_mode = false;
                //Debug.Log("Select mode: " + select_mode);
                //Debug.Log("Move mode: " + move_mode);
            }
        }

        /*-------------------------------------------------------------
        // Debug Stuff
        // Refresh the score and show the residues and bonds positions
        if (Input.GetKeyDown("p"))
        {
            //calculateResidueVariation(false);
            //calculateBondVariation(false);
            refreshScoreboard();
            calculateBestEnergy();
        }
        */
    }


    // Process the camera movements.
    private void LateUpdate()
    {
        if (select_mode)
        {
            // Camera movement
            // Movement in horizontal axis with joystick
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.RotateAround(camera_pivot.transform.position, transform.up, rotation_angle * Time.deltaTime * -Input.GetAxis("Horizontal"));
                //transform.Rotate(0.0f, 0.0f, rotation_angle * 2 * Time.deltaTime * -Input.GetAxis("Horizontal"));
            }
            // Movement in vertical axis with joystick
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.RotateAround(camera_pivot.transform.position, transform.right, rotation_angle * Time.deltaTime * Input.GetAxis("Vertical"));
            }
            // Zoom in/out with joystick buttons
            if (Input.GetAxis("Z-axis") != 0)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * zoom_smooth * Input.GetAxis("Z-axis"));
            }           
        }
    }


    // Process the residue movements.
    private void FixedUpdate()
    {
        // MOVE MODE
        // In this mode the player can:
        // Manipulate a residues position using the joystick input
        if (move_mode)
        {
            // Movement in horizontal axis (X axis)
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Vector3 translation = Vector3.right * delta * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
                target.transform.Translate(translation, Camera.main.transform);
                moved_flag = true;
            }
            // Movement in vertical axis (Y axis)
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                Vector3 translation = Vector3.up * delta * Input.GetAxisRaw("Vertical") * Time.deltaTime;
                target.transform.Translate(translation, Camera.main.transform);
                moved_flag = true;
            }
            // Movement in Z axis
            if (Input.GetAxisRaw("Z-axis") != 0)
            {
                Vector3 translation = Vector3.forward * delta * Input.GetAxisRaw("Z-axis") * Time.deltaTime;
                target.transform.Translate(translation, Camera.main.transform);
                moved_flag = true;
            }

            // Structure save state routine
            if (moved_flag)
            {
                moved_flag = false;
                // Remove all the next operations stored in undo_list
                if (undo_flag)
                {
                    undo_flag = false;
                    unlockStructure();
                    // Clear the redo stack
                    undo_list.RemoveRange(undo_index, (undo_list.Count - undo_index));
                }                              
                // Saves actual structure state
                if (!updating_score)
                {
                    StartCoroutine("updateScore");
                }
            }
        }
        // Undo/Redo operations
        else if (select_mode)
        {           
            // Undo
            if (Input.GetButton("B"))
            {
                if (undo_index > 1)
                {
                    undo_index--;
                    //Debug.Log("Undo index: " + undo_index);
                    // Return the strucutre to the previous configuration
                    loadPlayState(undo_list[undo_index - 1]);
                    lockStructure();
                    stabilizeStructure();
                    refreshScoreboard();
                    undo_flag = true;
                }
            }
            // Redo
            else if (Input.GetButton("A"))
            {
                if (undo_index < undo_list.Count)
                {
                    // Reapply the current movement                  
                    loadPlayState(undo_list[undo_index]);
                    //Debug.Log("Redo index: " + undo_index);
                    undo_index++;
                    lockStructure();
                    stabilizeStructure();
                    refreshScoreboard();
                    undo_flag = true;
                }
            }
        }
    }  


    /// <summary>
    /// Locks the residues and bonds setting the Rigidbody components as kinematic. 
    /// </summary>
    private void lockStructure()
    {
        foreach (var residue in particles)
            residue.GetComponent<Rigidbody>().isKinematic = true;

        foreach (var bond in bonds)
            bond.GetComponent<Rigidbody>().isKinematic = true;
    }


    /// <summary>
    /// Unlocks the residues and bonds setting the Rigidbody components as non-kinematic. 
    /// </summary>
    private void unlockStructure()
    {
        foreach (var residue in particles)
            residue.GetComponent<Rigidbody>().isKinematic = false;

        foreach (var bond in bonds)
            bond.GetComponent<Rigidbody>().isKinematic = false;
    }


    /// <summary>
    /// Stabilizes the structure residues positions, adds states to the undo/redo list and updates the scoreboard.
    /// </summary>
    /// <returns></returns>
    private IEnumerator updateScore()
    {
        updating_score = true;
        var energy = 0.0f;
        // stabilizes structure
        while (energy != potential_energy)
        {
            energy = potential_energy;
            //calculatePotentialEnergy();
            refreshScoreboard();
            insertState();
            //yield return null;
            yield return new WaitForSeconds(0.1f); 
        }
        yield return new WaitForSeconds(0.3f);
        refreshScoreboard();
        updating_score = false;
        insertState();
    }


    /// <summary>
    /// Runs simulation steps until the structure energy is stabilized.
    /// </summary>
    private void stabilizeStructure()
    {
        // Wait until the structure energy stabilizes
        Physics.autoSimulation = false;
        var energy = 0.0f;
        calculatePotentialEnergy();
        //var step = 0;
        do
        {
            energy = potential_energy;
            Physics.Simulate(Time.fixedDeltaTime);
            calculatePotentialEnergy();
            //Debug.Log("Step: " + ++step);
        } while (energy != potential_energy);

        Physics.autoSimulation = true;
    }


    /// <summary>
    /// Loads the Undo/Redo state to the structure objects.
    /// </summary>
    /// <param name="state">Structure state.</param>
    private void loadPlayState(PlayState state)
    {
        for (var i = 0; i < n_mol; i++)
        {
            particles[i].transform.position = state.residues_position[i];
            particles[i].transform.rotation = state.residues_rotation[i];
            if (i < n_mol - 1)
            {
                bonds[i].transform.position = state.bonds_position[i];
                bonds[i].transform.rotation = state.bonds_rotation[i];
            }
        }
    }


    /// <summary>
    /// Insert a structure state in the Undo/Redo list.
    /// </summary>
    private void insertState()
    {
        // Auxiliar arrays to get the residues and bonds positions
        Vector3[] residues_positon = new Vector3[n_mol];        
        Vector3[] bonds_position = new Vector3[n_mol - 1];
        Quaternion[] residues_rotation = new Quaternion[n_mol];
        Quaternion[] bonds_rotation = new Quaternion[n_mol - 1];
        // List isn't full
        if (undo_index < list_size)
        {
            // Get the residues and bonds current postions
            // Insert the positions state to the list (at the last list index used)
            undo_list.Insert(undo_index, getPlayState());
            undo_index++;
            //Debug.Log("Insert index: " + undo_index);
        }
        // List is full
        else
        {
            // Remove the first element of the list (in the bottom of the stack)
            undo_list.RemoveAt(0);
            // Get the residues and bonds current postions
            // Add the positions state to the list (in the top of the stack)
            undo_list.Add(getPlayState());
            //Debug.Log("Insert index: " + undo_index);
        }
    }


    /// <summary>
    /// Gets the state of the structure, to be stored in the Undo/Redo list.
    /// </summary>
    /// <returns>A PlayState object with the structure data.</returns>
    private PlayState getPlayState()
    {
        // Auxiliar arrays to get the residues and bonds positions and rotations
        Vector3[] residues_positon = new Vector3[n_mol];
        Vector3[] bonds_position = new Vector3[n_mol - 1];
        Quaternion[] residues_rotation = new Quaternion[n_mol];
        Quaternion[] bonds_rotation = new Quaternion[n_mol - 1];
        // Get the residues and bonds current postions         
        for (var i = 0; i < n_mol; i++)
        {
            residues_positon[i] = particles[i].transform.position;
            residues_rotation[i] = particles[i].transform.rotation;
            if (i < n_mol - 1)
            {
                bonds_position[i] = bonds[i].transform.position;
                bonds_rotation[i] = bonds[i].transform.rotation;
            }
        }
        return new PlayState(residues_positon, bonds_position, residues_rotation, bonds_rotation);
    }


    /// <summary>
    /// Blinks residue color periodically.
    /// </summary>
    /// <param name="rend">Residue renderer reference.</param>
    /// <param name="target_color">Start color.</param>
    /// <param name="color_blink">Final color.</param>
    private void blinkResidue(Renderer rend, Color target_color, Color color_blink)
    {
        float lerp = Mathf.PingPong(Time.time, blink_duration) / blink_duration;
        rend.material.color = Color.Lerp(target_color, color_blink, lerp);
    }


    /// <summary>
    /// Updates the structures parameters.
    /// </summary>
    private void refreshScoreboard()
    {
        calculatePotentialEnergy();
        calculateRg();
        setScoreText();
        setParametersText();
    }


    /// <summary>
    /// Calculates the Potential Energy using the 3D AB off-lattice model.
    /// </summary>
    private void calculatePotentialEnergy()
    {
        float u_bond = 0.0f;
        float u_torsion = 0.0f;
        float u_LJ = 0.0f;
        float r_ij;
        float u_LJ_pair;
        Vector3 dr1, dr2, dr3;

        /*
        for (var i = 0; i < n_mol - 2; i++)
        {
            dr1 = particles[i + 1].GetComponent<Transform>().transform.position - particles[i].GetComponent<Transform>().transform.position;
            dr2 = particles[i + 2].GetComponent<Transform>().transform.position - particles[i + 1].GetComponent<Transform>().transform.position;

            u_bond += Vector3.Dot(dr1, dr2);
        }
        */

        // Bond (partial) and Tosion Energy.
        for (var i = 0; i < n_mol - 3; i++)
        {
            dr1 = particles[i + 1].GetComponent<Transform>().transform.position - particles[i].GetComponent<Transform>().transform.position;
            dr2 = particles[i + 2].GetComponent<Transform>().transform.position - particles[i + 1].GetComponent<Transform>().transform.position;
            dr3 = particles[i + 3].GetComponent<Transform>().transform.position - particles[i + 2].GetComponent<Transform>().transform.position;

            u_bond += Vector3.Dot(dr1, dr2);
            u_torsion += (-0.5f) * Vector3.Dot(dr1, dr3);           
        }

        // Remaining Bond Energy.
        dr1 = particles[n_mol - 2].GetComponent<Transform>().transform.position - particles[n_mol - 3].GetComponent<Transform>().transform.position;
        dr2 = particles[n_mol - 1].GetComponent<Transform>().transform.position - particles[n_mol - 2].GetComponent<Transform>().transform.position;
        u_bond += Vector3.Dot(dr1, dr2);

        //Lennard-Jones Energy.
        for (var i = 0; i < n_mol - 2; i++)
        {
            for (var j = i + 2; j < n_mol; j++)
            {
                r_ij = Vector3.Distance(particles[i].GetComponent<Transform>().transform.position, particles[j].GetComponent<Transform>().transform.position);
                u_LJ_pair = 4.0f * (Mathf.Pow(r_ij, -12.0f) - Mathf.Pow(r_ij, -6.0f));
                if (sequence[i] != 'A' || sequence[j] != 'A')
                {
                    u_LJ_pair = 0.5f * u_LJ_pair;
                }
                u_LJ += u_LJ_pair;
            }
        }
        potential_energy = u_bond + u_torsion + u_LJ;
        bond_energy = u_bond;
        torsion_energy = u_torsion;
        lj_energy = u_LJ;
    }


    /// <summary>
    /// Calculates the Radius of Gyration parameters.
    /// </summary>
    private void calculateRg()
    {
        int h = 0;
        int p = 0;
        Vector3 avg = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 avg_h = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 avg_p = new Vector3(0.0f, 0.0f, 0.0f);

        rg_all = 0.0f;
        rg_h = 0.0f;
        rg_p = 0.0f;

        for (var i = 0; i < n_mol; i++)
        {
            avg += particles[i].GetComponent<Transform>().transform.position;

            if (sequence[i] == 'A')
            {
                avg_h += particles[i].GetComponent<Transform>().transform.position;
                h++;
            }
            else
            {
                avg_p += particles[i].GetComponent<Transform>().transform.position;
                p++;
            }
        }

        avg = avg / n_mol;
        avg_h = avg_h / h;
        avg_p = avg_p / p;

        for (var i = 0; i < n_mol; i++)
        {
            rg_all += (particles[i].GetComponent<Transform>().transform.position - avg).sqrMagnitude;

            if (sequence[i] == 'A')
            {
                rg_h += (particles[i].GetComponent<Transform>().transform.position - avg_h).sqrMagnitude;
            }
            else
            {
                rg_p += (particles[i].GetComponent<Transform>().transform.position - avg_p).sqrMagnitude;
            }
        }

        rg_all = Mathf.Sqrt(rg_all / n_mol);
        rg_h = Mathf.Sqrt(rg_h / h);
        rg_p = Mathf.Sqrt(rg_p / p);

        center_mass = avg;
    }


    /// <summary>
    /// Sets the player's graphical score.
    /// </summary>
    private void setScoreText()
    {
        // A good score must be a positive value.
        score = best_energy - potential_energy;
        //if (Mathf.Approximately(score, Mathf.Epsilon))
        if (score.ToString("F3").Equals("0,000") || score.ToString("F3").Equals("0.000"))
        {
            score = 0.0f;
            score_text.color = Color.black;
        }
        else
        {
            // Conditional expression. Orange if score is negative, green if score is positive.
            score_text.color = Mathf.Sign(score) < 0 ? orange_color : Color.green;
        }
        score_text.text = "Score: " + score.ToString("F3");
    }


    /// <summary>
    /// Sets the player's graphichal structures parameters.
    /// </summary>
    private void setParametersText()
    {
        string p_energy = "Potential Energy: " + potential_energy.ToString("F3");
        string rg_a = "Rg: " + rg_all.ToString("F3");
        string r_h = "Rg H: " + rg_h.ToString("F3");
        string r_p = "Rg P: " + rg_p.ToString("F3");
        parameters_text.text = p_energy + "\n" + rg_a + "\n" + r_h + "\n" + r_p;
    }


    /// <summary>
    /// Sets the camera pivot (center of rotation in camera movement).
    /// </summary>
    void setCameraPivot()
    {
        Vector3 avg = new Vector3(0.0f, 0.0f, 0.0f);
        for (var i = 0; i < n_mol; i++)
        {
            avg += particles[i].transform.position;
        }
        center_mass = avg / n_mol;
        camera_pivot.transform.position = center_mass;
    }


    /// <summary>
    /// Sets the Initial camera position to best fit the protein structure view.
    /// </summary>
    void setInitialPosition()
    {
        Vector3 avg = new Vector3(0.0f, 0.0f, 0.0f);
        for (var i = 0; i < n_mol; i++)
        {
            avg += particles[i].transform.position;
        }
        center_mass = avg / n_mol;
        // Set the camera pivot position of the sphere orbitation
        camera_pivot.transform.position = center_mass;
      
        // New Game camera initialization
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.New_game)))
        {
            // Calculate the distance to center mass until the further and closer residues
            float further = 0.0f;
            float closer = n_mol;
            Vector3 further_residue = Vector3.right;
            Vector3 closer_residue = Vector3.up;
            for (var i = 0; i < n_mol; i++)
            {
                var distance = Vector3.Distance(center_mass, particles[i].transform.position);
                if (distance > further)
                {
                    further = distance;
                    further_residue = particles[i].transform.position;
                }
                if (distance < closer)
                {
                    closer = distance;
                    closer_residue = particles[i].transform.position;
                }
            }
            
            // Calculate the perpendicular direction
            Vector3 player_position = Vector3.Cross(further_residue, closer_residue);
            player_position = player_position.normalized * further * 2.0f;
            
            // Move the position of the player (camera) to the calculated position
            transform.Translate(player_position);
            
            //Move the field of vision to see the structure
            transform.LookAt(camera_pivot.transform);
            Camera.main.transform.LookAt(camera_pivot.transform);
            Debug.Log("NEW GAME set initial position complete.");
        }
        // Load Game camera initialization
        else if (!string.IsNullOrEmpty(PlayerPrefs.GetString(GameFilesHandler.Saved_game)))
        {
            gameObject.transform.SetPositionAndRotation(camera_position, camera_rotation);
            //Debug.Log("Camera position: " + camera_position);
            //Debug.Log("Camera rotation: " + camera_rotation);
            //Debug.Log("Camera position is equal? " + Equals(camera_position, gameObject.transform.position));
            //Debug.Log("Camera rotation is equal? " + Equals(camera_rotation, gameObject.transform.rotation));
            Debug.Log("LOAD GAME set initial position complete.");
        }
    }



    //--- End of the game methods ----------------------------------------------------------------------------------------------------------------

    // DEBUD STUFF
    /*
    // Calculates the distance between two consecutive residues
    void calculateDistance()
    {
        Debug.Log("CALCULATE DISTANCE:");
        float distance;
        for (var i = 0; i < n_mol - 1; i++)
        {
            //distance = Vector3.Distance(particles[i].transform.position, particles[i + 1].transform.position);
            distance = Vector3.Distance(particles[i].GetComponent<Transform>().transform.position, particles[i + 1].GetComponent<Transform>().transform.position);
            //Debug.Log("Redidue " + i.ToString() + " = " + particles[i].transform.position.ToString("F8"));
            if (i == calc_distance)
            {
                Debug.Log("Distance particle " + i.ToString() + " to " + (i + 1).ToString() + " = " + distance.ToString("F8"));
            }
            //Debug.Log("Distance particle " + i.ToString() + " to " + (i + 1).ToString() + " = " + distance.ToString("F8"));
            //Debug.Log(particles[i].GetComponent<Transform>().transform.position.ToString("F8"));
            //Debug.Log(particles[i].transform.position.ToString("F8"));
        }
        //Debug.Log("Redidue " + (n_mol - 1).ToString() + " = " + particles[n_mol - 1].transform.position.ToString("F8"));
    }

    // Calculates the position variation of a residue, between its initial and actual position 
    void calculateResidueVariation(bool single)
    {
        Debug.Log("CALCULATE RESIDUES VARIATION:");

        if (single)
        {
            Debug.Log("R C[" + var_residue + "]: " + StructureInitialization.residues_coords[var_residue].ToString("F8"));
            Debug.Log("R P[" + var_residue + "]: " + particles[var_residue].GetComponent<Rigidbody>().transform.position.ToString("F8"));
            //var variation = Vector3.Distance(particles[i].transform.position, StructureInitialization.residues_coords[i]);
            var variation = StructureInitialization.residues_coords[var_residue] - particles[var_residue].transform.position;
            Debug.Log("Var R[" + var_residue + "]: " + variation.ToString("F8"));
        }
        else
        {
            for (var i = 0; i < n_mol; i++)
            {
              
                //Debug.Log("R C[" + i + "]: " + StructureInitialization.residues_coords[i].ToString("F8"));
                //Debug.Log("R P[" + i + "]: " + particles[i].GetComponent<Rigidbody>().transform.position.ToString("F8"));
                //var variation = Vector3.Distance(particles[i].transform.position, StructureInitialization.residues_coords[i]);
                var variation = StructureInitialization.residues_coords[i] - particles[i].transform.position;
                Debug.Log("Var R[" + i + "]: " + variation.ToString("F8"));
                //Debug.Log("Variation ResCoord[" + i + "]: " + variation.ToString("F8"));
                //Debug.Log("Var Variation Residue[" + i + "]: " + (variation - Vector3.Distance(StructureInitialization.res_coords[i], res_variation[i])).ToString("F8"));
                //Debug.Log("Pos Variation Residue[" + i + "]: " + Vector3.Distance(res_variation[i], particles[i].transform.position).ToString("F8"));
                //res_variation[i] = particles[i].transform.position;
            }
        }
    }
    
    // Calculates the position variation of a bond, between its initial and actual position
    void calculateBondVariation(bool single)
    {
        Debug.Log("CALCULATE BONDS VARIATION:");

        if (single)
        {
            Debug.Log("B C[" + var_bond + "]: " + StructureInitialization.bonds_coords[var_bond].ToString("F8"));
            Debug.Log("B P[" + var_bond + "]: " + StructureInitialization.bonds_structure[var_bond].GetComponent<Rigidbody>().transform.position.ToString("F8"));
            //var variation = Vector3.Distance(StructureInitialization.bonds_structure[i].transform.position, StructureInitialization.bonds_coords[i]);
            var variation = StructureInitialization.bonds_coords[var_bond] - StructureInitialization.bonds_structure[var_bond].transform.position;
            Debug.Log("Var B[" + var_bond + "]: " + variation.ToString("F8"));
        }
        else
        {
            for (var i = 0; i < n_mol - 1; i++)
            {
                //Debug.Log("B C[" + i + "]: " + StructureInitialization.bonds_coords[i].ToString("F8"));
                //Debug.Log("B P[" + i + "]: " + StructureInitialization.bonds_structure[i].GetComponent<Rigidbody>().transform.position.ToString("F8"));
                //var variation = Vector3.Distance(StructureInitialization.bonds_structure[i].transform.position, StructureInitialization.bonds_coords[i]);
                var variation = StructureInitialization.bonds_coords[i] - StructureInitialization.bonds_structure[i].transform.position;
                Debug.Log("Var B[" + i + "]: " + variation.ToString("F8"));

                //Debug.Log("Variation BondCoord[" + i + "]: " + variation.ToString("F8"));
                //Debug.Log("Var Variation Bond[" + i + "]: " + (variation - Vector3.Distance(StructureInitialization.bond_coords[i], bond_variation[i])).ToString("F8"));
                //Debug.Log("Pos Variation Bond[" + i + "]: " + Vector3.Distance(bond_variation[i], StructureInitialization.bond_structure[i].transform.position).ToString("F8"));
                //bond_variation[i] = StructureInitialization.bond_structure[i].transform.position;
            }
        }        
    }

    // Calculates the Energy of the structure
    private void calculateBestEnergy()
    {
        Vector3[] particles = StructureInitialization.residues_coords;
        float u_bond = 0.0f;
        float u_torsion = 0.0f;
        float u_LJ = 0.0f;
        float r_ij;
        float u_LJ_pair;
        Vector3 dr1, dr2, dr3;
        // Bond (partial) and Tosion Energy.
        for (var i = 0; i < n_mol - 3; i++)
        {
            dr1 = particles[i + 1] - particles[i];
            dr2 = particles[i + 2] - particles[i + 1];
            dr3 = particles[i + 3] - particles[i + 2];

            u_bond += Vector3.Dot(dr1, dr2);
            u_torsion += (-0.5f) * Vector3.Dot(dr1, dr3);
        }
        // Remaining Bond Energy.
        dr1 = particles[n_mol - 2] - particles[n_mol - 3];
        dr2 = particles[n_mol - 1] - particles[n_mol - 2];
        u_bond += Vector3.Dot(dr1, dr2);
        //Lennard-Jones Energy.
        for (var i = 0; i < n_mol - 2; i++)
        {
            for (var j = i + 2; j < n_mol; j++)
            {
                r_ij = Vector3.Distance(particles[i], particles[j]);
                u_LJ_pair = 4.0f * (Mathf.Pow(r_ij, -12.0f) - Mathf.Pow(r_ij, -6.0f));
                if (sequence[i] != 'A' || sequence[j] != 'A')
                {
                    u_LJ_pair = 0.5f * u_LJ_pair;
                }
                u_LJ += u_LJ_pair;
            }
        }
        //best_energy = u_bond + u_torsion + u_LJ;
        Debug.Log("Calc Best Energy = " + (u_bond + u_torsion + u_LJ).ToString("F12"));
        Debug.Log("Is Equal? " + string.Equals(best_energy.ToString(), (u_bond + u_torsion + u_LJ).ToString()));
    }
    */


    //----- End of PlayerController -------------------------------------------------------------------------------
}
