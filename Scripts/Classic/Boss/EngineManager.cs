using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EngineManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private Collider col;
    [SerializeField] private Slider hPSlider;
    public int engineNumber = 4;
    public UnityEvent engineEvent;
    public UnityEvent onHit;
    public int hP = 40;
    [SerializeField] private GameObject explo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hPSlider.value = hP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            hP--;
            onHit.Invoke();

            int hitSound = Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[hitSound]);
            
            Destroy(other.gameObject);
            if (hP  <= 0)
            {
                GameManager.instance.AddScore(2000);
                BossManager.instance.engineNumber--;
                ZeroHP();
            }
        }
    }

    public void ZeroHP()
    {
        
        Instantiate(explo, transform.position, transform.rotation);
        
        engineEvent.Invoke();
    }
}
