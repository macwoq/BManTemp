using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{

    public GameObject hitEffect;
    public float _speed;
    public Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1.5f);
        
    }

    // Update is called once per frame
    void Update()
    {        
        transform.Translate(Vector3.up * _speed * Time.deltaTime);   
        if(transform.position.z >= 14)
        {
            Destroy(this.gameObject);
        }
        //if(transform.position.z =< 15)
        //if(transform.position.z <= -1.5)
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Instantiate(hitEffect, transform.position, transform.rotation);

        }
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Enemy"){
            Destroy(this.gameObject);
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
    }
}
