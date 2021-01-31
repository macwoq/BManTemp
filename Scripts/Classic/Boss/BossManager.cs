using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bossShipObj;

    public int hP;
    public UnityEvent onHit;
    public UnityEvent bossShip;
    public UnityEvent engineHit;

    public int engineNumber = 4;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance is null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (engineNumber == 0)
        {
            engineHit.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            hP--;
            onHit.Invoke();
            Destroy(other.gameObject);
            if (hP <= 0)
            {
                DestroyBoss();
            }            
        }
    }

    public void DestroyBoss()
    {

        bossShip.Invoke();
    }


    public void EngineHit()
    {
        engineHit.Invoke();
    }


}
