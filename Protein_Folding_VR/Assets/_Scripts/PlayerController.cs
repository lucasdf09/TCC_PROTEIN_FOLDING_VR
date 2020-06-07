using System.Collections;
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
    public Transform camera_transform;      // Camera position and rotation reference 
    public GameObject menu_container;       // Menu container object reference
    public GameObject keyboard_container;   // Keyboard container object reference
    public GameObject structure;            // Structure object reference
    public float delta;                     // Residue translation rate
    public float blink_duration;            // Blink period increasing rate;

    public static bool select_mode;         // Flag to signal the select residue mode
    public static bool move_mode;           // Flag to signal the residue movement mode
    public static GameObject target;        // Gazed/Selected residue reference
    public static Color target_color;       // Target residue color reference
    public static float score;              // Diference between the Start Best Energy and the actual
    public static float saved_score;        // Score loaded from a saved file to set a new Best Energy start parameter
    public static float best_energy;        // Best Potential Energy reference for score calculation

    private static Color color_end;         // Color to blink with the original
    private static Color orange_color;      // Orange color;
    private GameObject[] particles;         // Reference to residues
    private int n_mol;                      // Number of molecules
    private Color color_aux;                // Auxiliar color object to store the residue own color during blink        
    private string sequence;                // AB Protein sequence 

    private float potential_energy;         // 3D AB off-lattice model Potential Energy 
    private float rg_all;                   // All residues Radius of Gyration (R.G.)
    private float rg_h;                     // Hydrophobic residues R.G. 
    private float rg_p;                     // Polar residues R.G.
    private Vector3 center_mass;            // Residues Center of Mass cartesian coordinates

    // DEBUG
    int calc_distance = 3;
    int var_residue = 3;
    int var_bond = 0;
    Vector3[] res_variation;                // Variation of the residues
    Vector3[] bond_variation;               // Variation of the bonds
    //float first_energy = 0.0f;
    private GameObject[] bonds;
    int counter;


    // Awake is called before Start().
    private void Awake()
    {
        // Initialization of attributes
        color_end = Color.yellow;
        orange_color = new Color(1.0f, 0.64f, 0.0f);
        score = 0.0f;
        best_energy = 0.0f;
        potential_energy = 0.0f;
        // Deactivate player interaction until initialization finish
        select_mode = false;
        move_mode = false;
    }

    private void Start()
    {
        target = null;
        particles = StructureInitialization.residues_structure;

        bonds = StructureInitialization.bonds_structure;

        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;

        Physics.autoSimulation = false;

        //calculatePotentialEnergy();
        //best_energy = potential_energy;
        //calculateRg();
        // Call initializeGame function in the next frame. 
        Invoke("initializeGame", 0);
    }

    /// <summary>
    /// Initialize the game and the player status
    /// </summary>
    private void initializeGame()
    {
        /*
        target = null;
        particles = StructureInitialization.residues_structure;

        bonds = StructureInitialization.bonds_structure;

        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;
        */

        //counter = ((n_mol * 2) - 1);
        //counter = 1;

        initializeParameters();
        setScoreText();
        setParametersText();
        setInitialPosition();
        // Activate the player interaction
        select_mode = true;
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
                Debug.Log("SELECT: CLICK key was pressed");
                // Get the residue own color
                color_aux = target_color;
                //Set the auxiliar color transparency to blink
                color_aux.a = 0.1f;
                select_mode = false;
                move_mode = true;
                Debug.Log("Select mode: " + select_mode);
                Debug.Log("Move mode: " + move_mode);
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
                menu_container.transform.position = camera_transform.position + camera_transform.forward * 2;
                menu_container.transform.rotation = camera_transform.rotation;
                menu_container.SetActive(true);
                // Set the Keyboard in front of the player view using the camera as reference
                keyboard_container.transform.position = camera_transform.position + camera_transform.forward * 2;                
                keyboard_container.transform.rotation = camera_transform.rotation;
                // Rotate the Keyboard to a better position for the player interaction
                keyboard_container.transform.Rotate(30, 0, 0);
            }
        }

        // Check the return condition to SELECT MODE, once in MOVE MODE
        else if (move_mode)
        {
            blinkResidue(target.GetComponent<Renderer>(), target_color, color_aux);
            // If the action button was pressed, return to select mode
            if (Input.GetButtonDown("C"))
            {
                print("MOVE: CLICK key was pressed");
                // Reset the residue own color
                target.GetComponent<Renderer>().material.color = target_color;
                target = null;
                reticle_pointer.SetActive(true);
                // Set the camera pivot after the residues movements
                setCameraPivot();               
                select_mode = true;
                move_mode = false;
                print("Select mode: " + select_mode);
                print("Move mode: " + move_mode);
            }
        }

        //***********************************************************************************************
        // Debug Stuff

        // Refresh the score and show the residues and bonds positions
        if (Input.GetKeyDown("p"))
        {
            //calculateResidueVariation(false);
            //calculateBondVariation(false);
            refreshScoreboard();
        }

        // Set isKinematic as False
        if (Input.GetKeyDown("k"))
        {         
            /*
            for (var i = 0; i < n_mol; i++)
                particles[i].GetComponent<Rigidbody>().isKinematic = false;

            for (var i = 0; i < n_mol-1; i++)
                bonds[i].GetComponent<Rigidbody>().isKinematic = false;
            */
        }

        // Set Rigidbody Constraints as None
        if (Input.GetKeyDown("c"))
        {
            foreach (var residue in StructureInitialization.residues_structure)            
                residue.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            foreach (var bond in StructureInitialization.bonds_structure)
                bond.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }      

        // Replace the residues to the original place
        if (Input.GetKeyDown("u"))
        {
            for (var i = 0; i < n_mol; i++)
            {              
                //particles[i].GetComponent<Rigidbody>().transform.Translate(StructureInitialization.residues_coords[i]);
                //particles[i].transform.Translate(StructureInitialization.residues_coords[i], Space.World); 
                //particles[i].GetComponent<Rigidbody>().transform.position = StructureInitialization.residues_coords[i];
                particles[i].transform.position = StructureInitialization.residues_coords[i];                
            }
        }
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
                //transform.RotateAround(camera_pivot.transform.position, Camera.main.transform.up, rotation_angle * Time.deltaTime * -Input.GetAxis("Horizontal"));
                //transform.Rotate(0.0f, 0.0f, rotation_angle * 2 * Time.deltaTime * -Input.GetAxis("Horizontal"));
            }
            // Movement in vertical axis with joystick
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.RotateAround(camera_pivot.transform.position, transform.right, rotation_angle * Time.deltaTime * Input.GetAxis("Vertical"));
                //transform.RotateAround(camera_pivot.transform.position, Camera.main.transform.right, rotation_angle * Time.deltaTime * Input.GetAxis("Vertical"));
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
                target.transform.Translate(Vector3.right * delta * Input.GetAxisRaw("Horizontal") * Time.deltaTime, Camera.main.transform);
            }
            // Movement in vertical axis (Y axis)
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                target.transform.Translate(Vector3.up * delta * Input.GetAxisRaw("Vertical") * Time.deltaTime, Camera.main.transform);
            }
            // Movemento in Z axis
            if (Input.GetAxisRaw("Z-axis") != 0)
            {
                target.transform.Translate(Vector3.forward * delta * Input.GetAxisRaw("Z-axis") * Time.deltaTime, Camera.main.transform);
            }
            refreshScoreboard();
        }
    }

    /// <summary>
    /// Blinks residue color periodically.
    /// </summary>
    /// <param name="rend">Residue renderer reference</param>
    /// <param name="target_color">Start color</param>
    /// <param name="color_blink">Final color</param>
    private void blinkResidue(Renderer rend, Color target_color, Color color_blink)
    {
        float lerp = Mathf.PingPong(Time.time, blink_duration) / blink_duration;
        rend.material.color = Color.Lerp(target_color, color_blink, lerp);
    }

    /// <summary>
    /// Initializes the structures parameters. 
    /// </summary>
    private void initializeParameters()
    {
        calculatePotentialEnergy();      
        best_energy = potential_energy;
        calculateRg();

        Physics.autoSimulation = true;

        //first_energy = potential_energy;
        //Physics.autoSimulation = false;
        //Debug.Log("Pre Simulation Step!");
        //var step = 0;

        //counter = ((n_mol * 2) - 1);
        //counter = 1;

        /*
        do
        {         
            best_energy = potential_energy;
            Physics.Simulate(Time.fixedDeltaTime);
            Debug.Log("Simulation Running");
            calculatePotentialEnergy();

            // Debug stuff
            //calculateDistance();
            Debug.Log("Step: " + ++step);
            calculateResidueVariation(true);
            //calculateBondVariation(); 
            
        } while (best_energy != potential_energy);
        */

        /*
        do
        {
            
            if ((counter % 2) != 0)
            {
                //particles[(counter / 2)].GetComponent<Rigidbody>().isKinematic = false;
                particles[(counter / 2)].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Debug.Log("Counter Res[" + (counter / 2) + "]");

            }
            if((counter % 2) == 0)
            {
                //bonds[(counter / 2) - 1].GetComponent<Rigidbody>().isKinematic = false;
                bonds[(counter / 2) - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Debug.Log("Counter Bond[" + (counter / 2 - 1) + "]");
            }
            
            if (counter > n_mol)
            {
                //bonds[counter - n_mol - 1].GetComponent<Rigidbody>().isKinematic = false;
                bonds[counter - n_mol - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Debug.Log("Counter Bond[" + (counter - n_mol - 1) + "]");
            }
            else if (counter <= n_mol)
            {
                //particles[counter - 1].GetComponent<Rigidbody>().isKinematic = false;
                particles[counter - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Debug.Log("Counter Res[" + (counter - 1) + "]");             
            }
            

            //counter--;
            //counter++;
            

            
            for (var i = 0; i < 20; i++)
            {
                Physics.Simulate(Time.fixedDeltaTime);
                Debug.Log("Step: " + ++step);
            }
            
            do
            {
                best_energy = potential_energy;
                for (var i = 0; i < 1; i++)
                    Physics.Simulate(Time.fixedDeltaTime);
                //calculatePotentialEnergy();
                refreshScoreboard();
                Debug.Log("Step: " + ++step);

            } while (best_energy != potential_energy);
            //refreshScoreboard();

        } while (counter > 0);
        //} while (counter < n_mol * 2);
        */


        //Physics.autoSimulation = true;
        //calculateRg();

        //best_energy = first_energy;

        /*
        for (var i = 0; i < n_mol; i++)
            particles[i].GetComponent<Rigidbody>().isKinematic = false;

        for (var i = 0; i < n_mol - 1; i++)
            bonds[i].GetComponent<Rigidbody>().isKinematic = false;
        */


        foreach (var residue in StructureInitialization.residues_structure)
            residue.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        foreach (var bond in StructureInitialization.bonds_structure)
            bond.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        Debug.Log("initializeParameters() Finished");
        //Debug.Log("Best Energy = " + best_energy.ToString("F12"));
        //Debug.Log("First Energy = " + first_energy.ToString("F12"));
        //Debug.Log("Potential Energy = " + potential_energy.ToString("F12"));

        //best_energy = potential_energy;

        // Debug stuff
        /*
        for (var i = 0; i < n_mol; i++)
        {
            //Debug.Log("Coord[" + i.ToString() + "] : " + StructureInitialization.residues_coords[i].ToString("F8"));
            //particles[i].GetComponent<Rigidbody>().transform.Translate(StructureInitialization.residues_coords[i]);
            //particles[i].transform.Translate(StructureInitialization.residues_coords[i], Space.World); 
            //particles[i].GetComponent<Rigidbody>().transform.position = StructureInitialization.residues_coords[i];
            particles[i].transform.position = StructureInitialization.residues_coords[i];
            Debug.Log("Position[" + i.ToString() + "] : " + particles[i].GetComponent<Rigidbody>().transform.position.ToString("F8"));
        }
        */
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
        if (score.ToString("F3").Equals("0,000"))
        {
            score = 0.0f;
            score_text.color = Color.white;
        }
        else
        {
            // Conditional expression. Orange if score is negative, green if score is positive.
            score_text.color = Mathf.Sign(score) < 0 ? orange_color : Color.green;
        }
        //score_text.text = "Score: " + score.ToString();
        score_text.text = "Score: " + score.ToString("F3");

        Debug.Log("Score Text:");
        Debug.Log("Best Energy = " + best_energy.ToString("F12"));
        Debug.Log("Potential Energy = " + potential_energy.ToString("F12"));
    }

    /// <summary>
    /// Sets the player's graphichal structures parameters.
    /// </summary>
    private void setParametersText()
    {
        string p_energy = "Potential Energy: " + potential_energy.ToString();
        string rg_a = "Rg: " + rg_all.ToString();
        string r_h = "Rg H: " + rg_h.ToString();
        string r_p = "Rg P: " + rg_p.ToString();
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
        //Debug.Log("Camera center mass: " + center_mass);
        //camera_pivot.transform.position = center_mass * Time.deltaTime * pivot_smooth;
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
        //Debug.Log("Player position vector: " + player_position);
        //Debug.Log("Camera center mass: " + center_mass);
        // Set the camera pivot position of the sphere orbitation
        camera_pivot.transform.position = center_mass;
        // Move the position of the player (camera) to the calculated position
        transform.Translate(player_position);
        //Debug.Log("Camera transform before: " + camera_transform.rotation);
        transform.LookAt(camera_pivot.transform);
        camera_transform.LookAt(camera_pivot.transform);
        //Debug.Log("Camera transform after: " + camera_transform.rotation);
    }


    // DEBUD STUFF

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

    // Calculates the position variation of a residue or a bond, between its initial and actual position 
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

// End of PlayerController
}
