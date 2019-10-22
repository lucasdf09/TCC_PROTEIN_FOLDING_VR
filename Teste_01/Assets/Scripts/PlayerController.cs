using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static bool select_mode;
    public static bool move_mode;
    public Text score_text;
    public Text parameters_text;

    private static GameObject target;
    private GameObject[] particles;
    private int n_mol;
    private int mol_count;
    private Color[] mol_colors;
    private Color color_end;
    private Color color_aux;
    private float duration;
    private Vector3 movement;
    private float delta;
    private string sequence;

    private float score;
    private float potential_energy;
    private float best_energy;
    private float rg_all;
    private float rg_h;
    private float rg_p;

    void Start()
    {
        target = StructureInitialization.mol_structure[1];
        particles = StructureInitialization.mol_structure;
        n_mol = StructureInitialization.n_mol;
        mol_count = 1;
        mol_colors = new Color[n_mol];
        for (var i = 0; i < n_mol; i++)
        {
            mol_colors[i] = particles[i].GetComponent<Renderer>().material.color;
        }
        color_end = Color.yellow;
        duration = 1.0f;
        //movement = new Vector3 (0.0f, 0.0f, 0.0f);
        delta = 0.01f;

        score = 0.0f;
        sequence = StructureInitialization.sequence;
        initializeParameters();
        setScoreText();
        setParametersText();
    }

    void Update()
    {
        if (select_mode) {
            if (Input.GetKeyDown("left"))
            {
                print("LEFT key was pressed");
                if (mol_count > 1)
                {
                    target.GetComponent<Renderer>().material.color = mol_colors[mol_count];
                    target = particles[--mol_count];
                }
                
            }
            else if (Input.GetKeyDown("right"))
            {
                print("RIGHT key was pressed");
                if (mol_count < n_mol - 1)
                {
                    target.GetComponent<Renderer>().material.color = mol_colors[mol_count];
                    target = particles[++mol_count];
                }              
            }
            else if (Input.GetKeyDown("return"))
            {
                print("ENTER key was pressed");
                color_aux = mol_colors[mol_count];
                color_aux.a = 0.1f;
                select_mode = false;
                move_mode = true;
                print("Select mode: " + select_mode);
                print("Move mode: " + move_mode);
                movement = target.GetComponent<Rigidbody>().transform.position;
            }

            blinkResidue(target.GetComponent<Renderer>(), color_end);
        }
        else if (move_mode)
        {
            /*
            if (Input.GetKey("w"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y += delta;
            }
            else if (Input.GetKey("s"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y -= delta;
            }
            if (Input.GetKey("d"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x += delta;
            }
            else if (Input.GetKey("a"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x -= delta;
            }
            if (Input.GetKey("e"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z += delta;
            }
            else if (Input.GetKey("q"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z -= delta;
            }

            target.GetComponent<Rigidbody>().transform.position = movement;
            */

            if (Input.GetKeyDown("return"))
            {
                print("ENTER key was pressed");
                select_mode = true;
                move_mode = false;
                print("Select mode: " + select_mode);
                print("Move mode: " + move_mode);
            }

            blinkResidue(target.GetComponent<Renderer>(), color_aux);
        }
    }

    
    private void FixedUpdate()
    {
        if (move_mode)
        {
            if (Input.GetKey("w"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y += delta;
            }
            else if (Input.GetKey("s"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.y -= delta;
            }
            if (Input.GetKey("d"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x += delta;
            }
            else if (Input.GetKey("a"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.x -= delta;
            }
            if (Input.GetKey("e"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z += delta;
            }
            else if (Input.GetKey("q"))
            {
                movement = target.GetComponent<Rigidbody>().transform.position;
                movement.z -= delta;
            }

            target.GetComponent<Rigidbody>().transform.position = movement;

            
            calculatePotentialEnergy();
            calculateRg();
            // A good score must be a positive value.
            score = best_energy - potential_energy;
            setScoreText();
            setParametersText();
            
        }
    }
    

    void blinkResidue(Renderer rend, Color color_blink)
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.color = Color.Lerp(mol_colors[mol_count], color_blink, lerp);
    }

    void initializeParameters()
    {
        calculatePotentialEnergy();
        best_energy = potential_energy;
        calculateRg();
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
    }

    void setScoreText()
    {
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
}
