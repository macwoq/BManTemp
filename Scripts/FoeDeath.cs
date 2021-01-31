using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeDeath : MonoBehaviour {

    public float deathTimer;
    public GameObject particle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            //if(particle)
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
