using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Shield instance;
    AudioSource audioSource;
    public AudioClip shieldUP;
    public AudioClip shieldDown;
    public AudioClip bulletImpact;

    public float shieldDuration;
    public GameObject shieldFX;
    public Vector3 scaleMax;
    public WaitForSeconds shieldDelay;

    public GameObject shieldLeft;
    public Vector3 leftScale;
    public GameObject shieldRight;
    public Vector3 rightScale;

    Collider col;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        transform.localScale = Vector3.zero;
        shieldDelay = new WaitForSeconds(shieldDuration);
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShield()
    {
        StartCoroutine(EngageShield());
    }

    IEnumerator EngageShield()
    {
        col.enabled = true;
        float inAnimDuration = 0.5f;
        float outAnimDuration = 0.5f;

        //shieldLeft.SetActive(true);
        //shieldRight.SetActive(true);

        while (inAnimDuration > 0f)
        {

            inAnimDuration -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, scaleMax, 0.1f);
            shieldLeft.transform.localScale = Vector3.Lerp(transform.localScale, leftScale, 0.5f);
            shieldRight.transform.localScale = Vector3.Lerp(transform.localScale, rightScale, 0.5f);
            yield return null;
        }


        //shieldFX.SetActive(true);
        yield return shieldDelay;

        while (outAnimDuration > 0f)
        {
            outAnimDuration -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
            shieldLeft.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.5f);
            shieldRight.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.5f);
            yield return null;
        }

        //shieldLeft.SetActive(false);
        //shieldRight.SetActive(false);

        transform.localScale = Vector3.zero;
        col.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag is "enemyBullet")
        {
            if (GameManager.instance.audioEnabled)
            {
                audioSource.PlayOneShot(bulletImpact);

            }
        }

        if(other.tag == "Enemy"){
            NewEnemyBeh.instance.hitPoints--;
        }
    }

}

