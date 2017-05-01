using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Destructor : MonoBehaviour
{

    private ParticleSystem par_sys;

    // Use this for initialization
    void Start ()
    {
        par_sys = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (par_sys)
        {
            if (!par_sys.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
