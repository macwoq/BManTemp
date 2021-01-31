using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enemyBehaviour3 : MonoBehaviour
{
    public float _enemy3Speed = 1;
    private Vector3 pos1;
    private Vector3 pos2;
    public Vector3 posDiff = new Vector3(20, 0, 0);
    public int _hp = 2;
    public GameObject boom;
    public UnityEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.position;
        pos2 = transform.position + posDiff;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time, _enemy3Speed));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            Debug.Log("hit nr3");
            _hp--;
            if (_hp == 0)
            {
                Invoke("enemyExplode", 2);
            }
        }
    }

    public void enemyExplode()
    {
        Instantiate(boom, transform.position, transform.rotation);
        Debug.Log("so far");
        onDeath.Invoke();
    }
}
