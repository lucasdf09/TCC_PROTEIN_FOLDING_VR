using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Text score_text;
    public Text parameters_text;
    public GameObject reticle_pointer;
    public GameObject camera_pivot;
    public float rotation_angle;
    public float zoom_factor;
    public float fov_min;
    public float fov_max;
    public float zoom_smooth;

    public static bool select_mode;
    public static bool move_mode;
    public static GameObject target;
    public static Color target_color;
    public static float best_energy;
    public static float score;
    public static float saved_score;

    private readonly Color color_end = Color.yellow;
    private readonly Color orange_color = new Color(1.0f, 0.64f, 0.0f);
    private const float blink_duration = 1.0f;
    private const float delta = 0.01f;

    private GameObject[] particles;
    private int n_mol;   
    private Color color_aux;   
    private Vector3 movement;  
    private string sequence;

    private float potential_energy;  
    private float rg_all;
    private float rg_h;
    private float rg_p;
    private Vector3 center_mass;

    private bool moved;

    // DEBUG
    Vector3[] res_variation;
    Vector3[] bond_variation;

    // Awake is called before Start(). Initializing attributes
    private void Awake()
    {
        score = 0.0f;
        best_energy = 0.0f;
        potential_energy = 0.0f;
        // Deactivate the player interaction
        select_mode = false;
        move_mode = false;
    }

    void Start()
    {
        Invoke("initializeGame", 0);
    }

    void initializeGame()
    {
        target = null;
        particles = StructureInitialization.residues_structure;
        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;
        moved = false;
        setScoreText();
        initializeParameters();
        setParametersText();
        setCameraPivot();
        // Activate the player interaction
        select_mode = true;

    }

    // Save game routine
    void saveGame(string save_file)
    {
        Debug.Log("Save key pressed!");
        gameObject.GetComponent<SaveHandler>().Save(save_file);
    }

    // Load game routine
    void loadGame(string load_file)
    {
        Debug.Log("Load key pressed!");
        // Deactivate player interaction
        reticle_pointer.SetActive(false);
        select_mode = false;
        move_mode = false;

        //Physics.autoSimulation = false;

        gameObject.GetComponent<StructureInitialization>().destroyStructure();
        gameObject.GetComponent<SaveHandler>().Load(load_file);
        gameObject.GetComponent<StructureInitialization>().loadStructure();

        target = null;
        particles = StructureInitialization.residues_structure;
        n_mol = StructureInitialization.n_mol;
        sequence = StructureInitialization.sequence;
        moved = false;
        refreshScoreboard();
        setCameraPivot();
        // Activate the player interaction
        select_mode = true;

        Debug.Log("Saved Score: " + saved_score.ToString("F8"));

        reticle_pointer.SetActive(true);

        //Physics.autoSimulation = true;

        //  Debug stuff
        //res_variation = new Vector3[n_mol];
        //bond_variation = new Vector3[n_mol - 1];
        //calculateDistance();
        //calculateVariation();
    }
    


    private void Update()
    {
        // SELECT MODE
        // In this mode the player can:
        // Select a residue to manipulate using the gaze input (reticle pointer);
        // Move the camera.
        if (select_mode) {

            // Check the transition to MOVE_MODE
            if (target != null && Input.GetButtonDown("Fire1"))
            {
                Debug.Log("SELECT: CLICK key was pressed");
                color_aux = target_color;
                color_aux.a = 0.1f;
                movement = target.GetComponent<Rigidbody>().transform.position;
                select_mode = false;
                move_mode = true;
                Debug.Log("Select mode: " + PlayerController.select_mode);
                Debug.Log("Move mode: " + PlayerController.move_mode);
                reticle_pointer.SetActive(false);

                refreshScoreboard();

                // DEBUG
                //calculateDistance();
                //calculateVariation();               
            }

            // Blink the residue on gaze
            if (target != null)
            {
                blinkResidue(target.GetComponent<Renderer>(), target_color, color_end);
            }

            // Save and Load test
            //Save
            if (Input.GetKeyDown("k"))
            {
                saveGame("/saveTest.josn");
            }
            //Load
            else if (Input.GetKeyDown("l"))
            {              
                loadGame("/saveTest.josn");              
            }
        }

        // Check the return condition to SELECT MODE, once in MOVE MODE
        else if (move_mode)
        {

            blinkResidue(target.GetComponent<Renderer>(), target_color, color_aux);

            // Put movement input here?

            //if (Input.GetKeyDown("return"))

            if (Input.GetButtonDown("Fire1"))
            {
                print("MOVE: CLICK key was pressed");
                target.GetComponent<Renderer>().material.color = target_color;
                target = null;
                reticle_pointer.SetActive(true);
                setCameraPivot();

                select_mode = true;
                move_mode = false;
                print("Select mode: " + select_mode);
                print("Move mode: " + move_mode);
            }
        }

        // DEBUG
        //refreshScoreboard();
        //calculateDistance();
        //calculateVariation();
    }

    // To process the camera movement
    private void LateUpdate()
    {
        if (select_mode)
        {
            /*** Camera movement ***/

            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.RotateAround(camera_pivot.transform.position, transform.up, rotation_angle * Time.deltaTime * -Input.GetAxis("Horizontal"));
                //transform.RotateAround(camera_pivot.transform.position, Camera.main.transform.up, rotation_angle * Time.deltaTime * -Input.GetAxis("Horizontal"));
                //transform.Rotate(0.0f, 0.0f, rotation_angle * 2 * Time.deltaTime * -Input.GetAxis("Horizontal"));
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.RotateAround(camera_pivot.transform.position, transform.right, rotation_angle * Time.deltaTime * Input.GetAxis("Vertical"));
                //transform.RotateAround(camera_pivot.transform.position, Camera.main.transform.right, rotation_angle * Time.deltaTime * Input.GetAxis("Vertical"));
            }
            if (Input.GetKey("i"))
            {
                transform.Translate(new Vector3(0,0,1) * Time.deltaTime * zoom_smooth);
                //cameraZoom(-1);
            }
            else if (Input.GetKey("o"))
            {
                transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * zoom_smooth);
                //cameraZoom(1);
            }
        }
    }

    // Like an image zoom, NOT IN USE
    private void cameraZoom(float signal)
    {
        float fov = Camera.main.fieldOfView;
        fov += zoom_factor * signal;
        fov = Mathf.Clamp(fov, fov_min, fov_max);
        //Camera.main.fieldOfView = fov;
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, Time.deltaTime * zoom_smooth);
    }

    private void FixedUpdate()
    {
        // MOVE MODE
        // In this mode the player can:
        // Manipulate a residues position using the joystick input; 
        if (move_mode)
        {
            if (Input.GetKey("w"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y += delta;
                moved = true;
            }
            else if (Input.GetKey("s"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y -= delta;
                moved = true;
            }
            if (Input.GetKey("d"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x += delta;
                moved = true;
            }
            else if (Input.GetKey("a"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x -= delta;
                moved = true;
            }
            if (Input.GetKey("e"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z += delta;
                moved = true;
            }
            else if (Input.GetKey("q"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z -= delta;
                moved = true;
            }

            if (moved)
            {
                target.GetComponent<Rigidbody>().transform.position = movement;
                moved = false;
            }
      
            //calculatePotentialEnergy();
            //calculateRg();
            //setScoreText();
            //setParametersText();

            refreshScoreboard();
        }
    }
    

    void blinkResidue(Renderer rend, Color target_color, Color color_blink)
    {
        float lerp = Mathf.PingPong(Time.time, blink_duration) / blink_duration;
        rend.material.color = Color.Lerp(target_color, color_blink, lerp);
    }

    void initializeParameters()
    {
        calculatePotentialEnergy();
        float first_energy = potential_energy;
        Physics.autoSimulation = false;
        Debug.Log("Pre Simulation Step!");
        do
        {
            best_energy = potential_energy;
            Physics.Simulate(Time.fixedDeltaTime);
            Debug.Log("Simulation Step!");
            calculatePotentialEnergy();
        } while (best_energy != potential_energy);
        Physics.autoSimulation = true;
        calculateRg();
        Debug.Log("initializeParameters() Finished.\n Best_energy = " + best_energy + "\nFirst_energy = " + first_energy);
    }

    void refreshScoreboard()
    {
        calculatePotentialEnergy();
        calculateRg();
        setScoreText();
        setParametersText();
    }

    void calculatePotentialEnergy()
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

    void calculateRg()
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

    void setScoreText()
    {
        // A good score must be a positive value.
        score = best_energy - potential_energy;
        if (Mathf.Approximately(score, Mathf.Epsilon))
        {
            score = 0.0f;
            score_text.color = Color.white;
        }
        else
        {
            // Conditional expression. Orange if score is negative, green if score is positive.
            score_text.color = Mathf.Sign(score) < 0 ? orange_color : Color.green;
        }
        score_text.text = "Score: " + score.ToString();
    }

    void setParametersText()
    {
        string p_energy = "Potential Energy: " + potential_energy.ToString();
        string rg_a = "Rg: " + rg_all.ToString();
        string r_h = "Rg H: " + rg_h.ToString();
        string r_p = "Rg P: " + rg_p.ToString();
        parameters_text.text = p_energy + "\n" + rg_a + "\n" + r_h + "\n" + r_p;
    }


    void setCameraPivot()
    {
        Vector3 avg = new Vector3(0.0f, 0.0f, 0.0f);

        for (var i = 0; i < n_mol; i++)
        {
            avg += particles[i].transform.position;
        }

        center_mass = avg / n_mol;

        Debug.Log("Camera center mass: " + center_mass);

        camera_pivot.transform.position = center_mass;
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
            Debug.Log("Bond " + i.ToString() + " = " + distance.ToString("F8"));
            //Debug.Log(particles[i].GetComponent<Transform>().transform.position.ToString("F8"));
            //Debug.Log(particles[i].transform.position.ToString("F8"));
        }
        //Debug.Log("Redidue " + (n_mol - 1).ToString() + " = " + particles[n_mol - 1].transform.position.ToString("F8"));
    }

    // Calculates the position variation of a residue or a bond, between its initial and actual position 
    void calculateVariation()
    {
        Debug.Log("CALCULATE VARIATION:");
        float variation;


        // Residues variation
        //Debug.Log("Residue Variation:");
        for (var i = 0; i < n_mol; i++)
        {
            //Debug.Log("ResiduePos[" + i + "]: " + particles[i].GetComponent<Rigidbody>().transform.position.ToString("F8"));
            //Debug.Log("ResidueCoords[" + i + "]: " + StructureInitialization.res_coords[i].ToString("F8"));         
            variation = Vector3.Distance(particles[i].transform.position, StructureInitialization.residues_coords[i]);
            Debug.Log("Variation ResCoord[" + i + "]: " + variation.ToString("F8"));
            //Debug.Log("Var Variation Residue[" + i + "]: " + (variation - Vector3.Distance(StructureInitialization.res_coords[i], res_variation[i])).ToString("F8"));
            //Debug.Log("Pos Variation Residue[" + i + "]: " + Vector3.Distance(res_variation[i], particles[i].transform.position).ToString("F8"));
            //res_variation[i] = particles[i].transform.position;

        }
        // Bonds variation
        //Debug.Log("Bond Variation:");
        for (var i = 0; i < n_mol - 1; i++)
        {
            //Debug.Log("BondPos[" + i + "]: " + StructureInitialization.bond_structure[i].GetComponent<Rigidbody>().transform.position.ToString("F8"));
            //Debug.Log("BondCoords[" + i + "]: " + StructureInitialization.bond_coords[i].ToString("F8"));
            variation = Vector3.Distance(StructureInitialization.bonds_structure[i].transform.position, StructureInitialization.bonds_coords[i]);
            Debug.Log("Variation BondCoord[" + i + "]: " + variation.ToString("F8"));
            //Debug.Log("Var Variation Bond[" + i + "]: " + (variation - Vector3.Distance(StructureInitialization.bond_coords[i], bond_variation[i])).ToString("F8"));
            //Debug.Log("Pos Variation Bond[" + i + "]: " + Vector3.Distance(bond_variation[i], StructureInitialization.bond_structure[i].transform.position).ToString("F8"));
            //bond_variation[i] = StructureInitialization.bond_structure[i].transform.position;
        }
    }
}
