using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Lightning_Shooter : MonoBehaviour {

    public VRTK_StraightPointerRenderer_Custom point_renderer;
    public VRTK_ControllerEvents Events;

    [Range(0, 12)]
    [Tooltip("The number of segments used for a lightning bolt.")]
    public int Generations = 6;

    [Range(0.01f, 1.0f)]
    [Tooltip("How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
    public float Duration = 0.05f;

    [Range(0.0f, 1.0f)]
    [Tooltip("How extreme the offsets for each segment should be")]
    public float manual_Offset = 0.15f;

    [Range(0, 100)]
    [Tooltip("Percent chance that the bolt can fork. Warning: Bolts with a high generation count have a large number of chances to fork")]
    public float fork_chance = 1.0f;

    [Tooltip("Whether the offset should decrease over the distance of the bolt")]
    public bool decrease_w_dist;

    [Tooltip("If true, this bolt can arc to objects in the list")]
    public bool arc;

    [Tooltip("List of objects the bolt can arc too.")]
    public List<GameObject> arc_list = new List<GameObject>();


    // Use this for initialization
    void Start ()
    {
       // renderer = GetComponent<VRTK_StraightPointerRenderer>();
       // Events = GetComponent<VRTK_ControllerEvents>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Events.triggerClicked == true)
        {
            //Vector3 end_pos = point_renderer.actualCursor.transform.position;

            Vector3 end_pos = point_renderer.returnCursorPos();

           // Vector3 end_pos = new Vector3(0.0f, 0.0f, 0.0f);

            GameObject new_bolt = (GameObject)Instantiate(Resources.Load("Lightning_Bolt"));

            Debug.Log("Fired a bolt");

            new_bolt.GetComponent<Lightning_Custom>().Initiate(null, transform.position, null,
                end_pos, Generations, Duration, manual_Offset, decrease_w_dist,
                false, fork_chance, false, arc, arc_list);

            //manually summon a bolt because it is not set to automatic.
            new_bolt.GetComponent<Lightning_Custom>().Summon();
        }
	}
}
