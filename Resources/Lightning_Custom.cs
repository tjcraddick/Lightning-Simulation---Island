using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Custom : MonoBehaviour
{
    //Public variables, all adjustable from editor
    [Tooltip("If empty, then startposition will be used, otherwise the result will be the object position plus the startposition")]
    public GameObject start_obj;

    [Tooltip("The start position where the lightning will emit from in world space coordinates")]
    public Vector3 start_position;

    [Tooltip("If empty, then endposition will be used, otherwise the result will be the object position plus the endposition")]
    public GameObject end_obj;

    [Tooltip("The end position where the lightning will end at in worldspace coordinates.")]
    public Vector3 end_position;

    [Range(0, 12)]
    [Tooltip("The number of segments used for a lightning bolt.")]
    public int Generations = 6;

    [Range(0.01f, 1.0f)]
    [Tooltip("How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
    public float Duration = 0.05f;
    private float timer = 0.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("How extreme the offsets for each segment should be")]
    public float manual_Offset = 0.15f;
    private float original_offset;

    [Tooltip("Whether the offset should decrease over the distance of the bolt")]
    public bool decrease_w_dist;

    [Tooltip("If true, automatically generates bolts as soon as timer finishes")]
    public bool autogenerate;

    [Range(0, 100)]
    [Tooltip("Percent chance that the bolt can fork. Warning: Bolts with a high generation count have a large number of chances to fork")]
    public float fork_chance = 1.0f;

    [Tooltip("If true, this bolt is an offshoot")]
    public bool offshoot;

    [Tooltip("If true, this offshoots of this bolt can arc to objects in the list")]
    public bool arc;

    [Tooltip("List of objects the bolt can arc too.")]
    public List<GameObject> arc_list = new List<GameObject>();


    //Private Variables, not accessible from editor.
    struct segment
    {
        public Vector3 Start;
        //public Vector3 Midpoint;
        public Vector3 End;



        public segment(Vector3 start, Vector3 end)
        {
            Start = start;
            // Midpoint = mid;
            End = end;
        }

    };


    // private System.Random Rand_Gen = new System.Random();
    private Vector3 empty;

    // List<List<segment>> Bolts = new List<List<segment>>();
    List<segment> seg_lines = new List<segment>();
    private LineRenderer line_rend;

    private int final_index;
    private int total_segments;



    public void Initiate(GameObject start_obj_in, Vector3 start_pos_in,
        GameObject end_obj_in, Vector3 end_pos_in, int generations_in,
        float duration_in, float offset_in, bool decrease_in,
        bool auto_in, float fork_chance_in, bool offshoot_in, 
        bool arc_in, List<GameObject> list_in)
    {
        start_obj = start_obj_in;
        start_position = start_pos_in;
        end_obj = end_obj_in;
        end_position = end_pos_in;
        Generations = generations_in;
        Duration = duration_in;
        manual_Offset = offset_in;
        decrease_w_dist = decrease_in;
        autogenerate = auto_in;
        fork_chance = fork_chance_in;
        offshoot = offshoot_in;
        arc = arc_in;
        arc_list = list_in;

        line_rend = GetComponent<LineRenderer>();
<<<<<<< HEAD
        line_rend.numPositions = 0;
=======
        line_rend.positionCount = 0;
>>>>>>> origin/Scene_Terrain
        empty = new Vector3(0.0f, 0.0f, 0.0f);

        final_index = 0;
        total_segments = 0;
        original_offset = manual_Offset;
    }


    // Use this for initialization
    void Start()
    {
        if(autogenerate == true)
        {
            line_rend = GetComponent<LineRenderer>();
<<<<<<< HEAD
            line_rend.numPositions = 0;
=======
            line_rend.positionCount = 0;
>>>>>>> origin/Scene_Terrain
            empty = new Vector3(0.0f, 0.0f, 0.0f);

            final_index = 0;
            total_segments = 0;
            original_offset = manual_Offset;
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool offshoot_called = false;

        if (timer <= 0.0f)
        {
            if (!autogenerate)
            {
                timer = Duration;
<<<<<<< HEAD
                line_rend.numPositions = 0;
=======
                line_rend.positionCount = 0;
>>>>>>> origin/Scene_Terrain

                //attempt: delete all non automatic bolts at the end. Assumes
                //that any manual bolts will be created objects.
                Destroy(this.gameObject);
            }
            else
            {
                if (offshoot_called == false)
                {
                    Summon();
                }
                if (offshoot == true)
                {
                    //Debug.Log("wat");
                    
                    offshoot_called = true;
                }
                // Summon();

            }
        }
        timer -= Time.deltaTime;
    }

    void generate_Bolt(Vector3 startpos, Vector3 endpos, int num_generations)
    {


        seg_lines.Add(new segment(startpos, endpos));

        Vector3 Dist;
        Vector3 start;
        Vector3 mid;
        Vector3 end;
        Vector3 offset_vec;

        Vector3 offset_dist = endpos - startpos;

        float offset = offset_dist.magnitude * manual_Offset;

        int curr_index = 0;
        int next_index = seg_lines.Count;


        for (int i = 0; i < num_generations; i++)
        {
            //Debug.Log("Generation: " + num_generations + " -- i: " + i);

            for (int j = curr_index; j < next_index; j++)
            {
                start = seg_lines[j].Start;
                end = seg_lines[j].End;

                Dist = end + start; 
                mid = Dist * 0.5f;

                offset_vec = vector_offset(start, end, offset, j);

                mid = mid + offset_vec;


                seg_lines.Add(new segment(start, mid));
                seg_lines.Add(new segment(mid, end));


            }
            offset *= Random.Range(0.5f, 1.0f); //How much the lightning offset should decrease
            curr_index = next_index;
            next_index = seg_lines.Count;

            if (i == num_generations - 1) // Final pass
            {
                total_segments = next_index - curr_index;
                final_index = curr_index;
            }
        }
    }

    Vector3 vector_offset(Vector3 start, Vector3 end, float offset_amount, int curr_pos)
    {
        Vector3 offset_vec = new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 dir = end - start;
        dir = dir.normalized;

        float randx, randy, randz;
        randx = Random.Range(0.0f, 1.0f);
        randy = Random.Range(0.0f, 1.0f);
        randz = Random.Range(0.0f, 1.0f);
        Vector3 rand_vec = new Vector3(randx, randy, randz);

        Vector3 orthogonal = Vector3.Cross(dir, rand_vec);

        float offset_dist = 0.0f;

        if (decrease_w_dist == true)
        {
            //curr_pos, or j, is always less than seg_lines.Count, so this should never hit an error
            offset_dist = Random.Range(0.0f, 0.5f) * offset_amount * (seg_lines.Count / (seg_lines.Count - curr_pos) );
        }
        else
        {
            offset_dist = Random.Range(0.0f, 0.5f) * offset_amount;
        }
        
        float offset_rot = Random.Range(0.0f, 5.0f) * 360.0f;
        offset_vec = Quaternion.AngleAxis(offset_rot, dir) * orthogonal * offset_dist;


        return offset_vec;
    }


    // Summons a lightning bolt.
    public void Summon()
    {
        Vector3 start, end;
        timer = Duration;

        if (start_obj == null)
        {
            start = start_position;
        }
        else
        {
            start = start_obj.transform.position + start_position;
        }
        if (end_obj == null)
        {
            end = end_position;
        }
        else
        {
            end = end_obj.transform.position + end_position;
        }


        generate_Bolt(start, end, Generations);

        //Updating the Unity Line Renderer
<<<<<<< HEAD
        line_rend.numPositions = total_segments + 1;
=======
        line_rend.positionCount = total_segments + 1;
>>>>>>> origin/Scene_Terrain

        int index_pos = 0;
        //add the starting point to the list of line positions.


        line_rend.SetPosition(index_pos, seg_lines[final_index].Start);

        index_pos++;


        for (int i = final_index; i < seg_lines.Count; i++)
        {
            //starting with the first midpoint, add each subsequent end point
            //to the line position list.
            line_rend.SetPosition(index_pos, seg_lines[i].End);
            index_pos++;

            if (Random.Range(0.0f, 100.0f) >= (100 - fork_chance) )
            {              
                GameObject new_bolt = (GameObject)Instantiate(Resources.Load("Lightning_Bolt"));
                int link_pos;
                Vector3 new_end = empty;

                //Determines whether the split bolt goes to a random position or links back up with the main bolt
                float linked = Random.Range(0.0f, 1.0f);

                if(linked > 0.5f) //links back up with the main bolt.
                {
                    if(i +1 < seg_lines.Count)
                    {
                        link_pos = Random.Range(i + 1, seg_lines.Count);
                        new_end = seg_lines[link_pos].End;
                    }

                }
                else //random position nearby, or the player if set and nearby.
                {
                    float new_x = Random.Range(0.0f, 2.0f) + seg_lines[i].Start.x;
                    float new_y = Random.Range(0.0f, 2.0f) + seg_lines[i].Start.y;
                    float new_z = Random.Range(0.0f, 2.0f) + seg_lines[i].Start.z;
                    new_end = new Vector3(new_x, new_y, new_z);

                    if(arc == true)
                    {
                        int smallest_index = -1; //-1 indicates nothing within range

                        if(arc_list.Count > 0)
                        {
                            float temp_dist = 3.0f; //object must be within 2 units maximum
                            for (int j = 0; j < arc_list.Count; j++)
                            {
                                float dist = (arc_list[j].GetComponent<Transform>().position - seg_lines[i].Start).magnitude;
                                if(dist < temp_dist)
                                {
                                    temp_dist = dist;
                                    smallest_index = j;
                                }
                            }

                            if(smallest_index != -1)
                            {
                                new_end = arc_list[smallest_index].GetComponent<Transform>().position;
                            }


                        }
                    }

                }

               // Debug.Log("Duration: " + Duration);
                new_bolt.GetComponent<Lightning_Custom>().Initiate(null, seg_lines[i].Start, null,
                    new_end, 3, timer, Random.Range(0.15f,1.0f), decrease_w_dist, 
                    false, fork_chance, true, arc, arc_list);

                //manually summon a bolt because it is not set to automatic.
                new_bolt.GetComponent<Lightning_Custom>().Summon();
                    

            }

        }

        seg_lines.Clear();

    }
}
