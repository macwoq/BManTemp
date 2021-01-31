using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enemyBehaviour2 : MonoBehaviour
{
    public float _enemy2Speed = 1;
    private Vector3 pos1;
    private Vector3 pos2;
    public Vector3 posDiff = new Vector3(10, 0, 0);
    public int _hp = 2;
    public GameObject boom;
    public GameObject invader2;
    private Collider col;
    public UnityEvent onDeathEvents;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.position;
        pos2 = transform.position + posDiff;
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time, _enemy2Speed));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {

            Debug.Log("hit nr2");
            _hp--;
            if (_hp == 0)
            {
                Invoke("enemyExplode", 0.1f);
            }
        }
    }

    public void enemyExplode()
    {
        onDeathEvents.Invoke();
        Instantiate(boom, transform.position, transform.rotation);
        invader2.SetActive(false);
        col.enabled = false;
        gameObject.SetActive(false);
    }
}
