using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyLaser : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject hitMuzzle;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Classic_PlayerBehaviour.instance.TakeDamage(-1);
            GameManager.instance.GetHit();
            Destroy(this.gameObject);
            Instantiate(hitMuzzle, transform.position, transform.rotation);
            
        }

    }
}