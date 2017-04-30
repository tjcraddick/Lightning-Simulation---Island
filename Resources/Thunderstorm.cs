using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunderstorm : MonoBehaviour
{
    [Range(0.0f, 500.0f)]
    [Tooltip("The radius of the circle in which bolts an spawn")]
    public float radius = 1.0f;

    [Tooltip("How many seconds between new bolts")]
    public int delay = 5;

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

    [Tooltip("Whether the offset should decrease over the distance of the bolt")]
    public bool decrease_w_dist;

    [Tooltip("If true, this bolt can arc to objects in the list")]
    public bool arc;

    [Tooltip("List of objects the bolt can arc too.")]
    public List<GameObject> arc_list = new List<GameObject>();

    public GameObject Thundercloud;
    public GameObject Sparks;
    public GameObject Terrain;

    private float cloud_height;
    private bool has_fired;

	// Use this for initialization
	void Start ()
    {
        cloud_height = transform.position.y;
        has_fired = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
       // Debug.Log(Time.time);
        if (Mathf.Floor(Time.time) % delay == 0 && has_fired == false) //Summon a new bolt
        {
            new_cloud();
            has_fired = true;
        }
        
        if(Mathf.Floor(Time.time) % delay != 0 && has_fired == true)
        {
            has_fired = false;
        }
	}


    void new_cloud()
    {
        float x_pos = Random.Range(-radius, radius);
        float z_pos = Random.Range(-radius, radius);

        Vector3 cloud_pos = new Vector3(x_pos, cloud_height, z_pos);
        Vector3 final_pos = new Vector3(x_pos, 0.0f, z_pos);

        float height_dist = cloud_height; //assuming ground is 0.0 for now

        if (arc == true)
        {
            int smallest_index = -1; //-1 indicates nothing within range

            if (arc_list.Count > 0)
            {

                for (int i = 0; i < arc_list.Count; i++)
                {
                    float dist = (arc_list[i].GetComponent<Transform>().position - transform.position).magnitude;
                    if (dist < height_dist)
                    {
                        height_dist = dist;
                        smallest_index = i;
                    }
                }

                if (smallest_index != -1)
                {
                    final_pos = arc_list[smallest_index].GetComponent<Transform>().position;
                }


            }
        }

        GameObject new_bolt = (GameObject)Instantiate(Resources.Load("Lightning_Bolt"));

        new_bolt.GetComponent<Lightning_Custom>().Initiate(null, cloud_pos, null,
            final_pos, Generations, Duration, manual_Offset, decrease_w_dist,
            false, false, arc, arc_list);

        //manually summon a bolt because it is not set to automatic.
        new_bolt.GetComponent<Lightning_Custom>().Summon();

        //GameObject cloud = (GameObject)Instantiate(Resources.Load("Thundercloud"));
        //cloud.transform.position.Set(cloud_pos.x, cloud_pos.y, cloud_pos.z);

        if(Thundercloud != null)
        {
            Instantiate(Thundercloud, cloud_pos, Quaternion.Euler(0, 0, 0));
        }
        if(Terrain != null)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(final_pos.x, 100000.0f, final_pos.z), Vector3.down);

            if(Terrain.GetComponent<Collider>().Raycast(ray, out hit, 1000000.0f))
            {
                Debug.Log("Hit point: " + hit.point);
                final_pos.y = hit.point.y;
            }


        }
        if(Sparks != null && Terrain != null)
        {
            Instantiate(Sparks, final_pos, Quaternion.Euler(0, 0, 0));
        }

    }


}
