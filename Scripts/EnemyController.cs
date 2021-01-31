using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public AudioSource shotSound;
    public AudioClip enemyShot;
    public AudioClip explosion;
    public AudioClip _playerTouch;
    public float speed;
    public float changeTimer;
    public bool directionSwitch;
    public float shootPower;
    public int hP;
    public GameObject powerUp;
    public GameObject powerDown;
    //public GameObject healthUp;
    public GameObject particleEffect;
    public GameObject deathParticle;
    public MapLimits Limits;
    public GameObject bullet;
    public float maxTimer;
    public bool canShoot;
    public Transform shootPos;
    float shootTimer;
    float maxShootTimer;
    public int scoreReward;

    Rigidbody rb;
	// Use this for initialization
	void Start () {

        shootTimer = Random.Range(1, 10);
        maxShootTimer = shootTimer;
        
        maxTimer = changeTimer;
        rb = GetComponent<Rigidbody>();
        shotSound.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        switchTimer();
        if (transform.position.x ==18) switchDir(directionSwitch);
        if (transform.position.x == Limits.minimumX) switchDir(directionSwitch);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, Limits.minimumX, Limits.maximumX),
             Mathf.Clamp(transform.position.y, Limits.minimumY, Limits.maximumY), 0.0f);

        shootTimer -= Time.deltaTime;
        if (canShoot)
        if (shootTimer <= 0)
        {
           GameObject newBullet = Instantiate(bullet, shootPos.transform.position, Quaternion.Euler(0,0,0));
            newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * -shootPower;
                shotSound.PlayOneShot(enemyShot);
                shootTimer = maxShootTimer;
                 
        }

    }


    public void Movement()
    {
        if (directionSwitch)
            rb.velocity = new Vector3(speed, -speed, 0 * Time.deltaTime);
        else
            rb.velocity = new Vector3(-speed, -speed, 0 * Time.deltaTime);
    }

    public void switchTimer()
    {
        
        changeTimer -= Time.deltaTime;
        if (changeTimer < 0)
        {
            switchDir(directionSwitch);
            changeTimer = maxTimer;


        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag== "friendlyBullet")
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            hP--;

            EnemyStats stats;

            if (stats = GetComponent<EnemyStats>())
            {
                stats.ChangeHealth(-1);
            }

            


            if (hP <= 0)
            {
                //RFX4_CameraShake shake;
                //shake = GetComponent<RFX4_CameraShake>();
                //shake.PlayShake();
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 30) Instantiate(powerUp, transform.position, transform.rotation);
                //if (randomNumber < 15) Instantiate(healthUp, transform.position, transform.rotation);
                if (randomNumber > 80) Instantiate(powerDown, transform.position, transform.rotation);
                RFX4_CameraShake.Instance.PlayShake();
                GameObject obj = GameObject.Find("ExplodeAudio");
                AudioSource aud = obj.GetComponent<AudioSource>();
                aud.PlayOneShot(explosion);
                Instantiate(deathParticle, transform.position, transform.rotation);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().score += scoreReward;
                Destroy(gameObject);

            }

        }
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerCharacter>().hP--;
            hP--;
            GameObject obj2 = GameObject.Find("TouchAudio");
            AudioSource aud2 = obj2.GetComponent<AudioSource>();
            aud2.PlayOneShot(_playerTouch);
            if (hP <= 0)
            {
                RFX4_CameraShake.Instance.PlayShake();
                GameObject obj = GameObject.Find("ExplodeAudio");
                AudioSource aud = obj.GetComponent<AudioSource>();
                aud.PlayOneShot(explosion);
                Instantiate(deathParticle, transform.position, transform.rotation);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().score += scoreReward;
                Destroy(gameObject);
            }
                
        }
    }

    private void addScore()
    {
        
    }

    bool switchDir(bool dir)
    {
        if (dir) directionSwitch = false;
        else directionSwitch = true;
        return dir;
    }

 
}
