using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class NewEnemyBeh : MonoBehaviour
{

    public static NewEnemyBeh instance;

    public Path pathToFollow;

    //path infos
    [Header("On hit & On Death Events")]
    public GameObject[] explosionsEffects;
    public UnityEvent onDeathEvent;
    public GameObject boomEfect;
    private AudioSource audioSource;
    public AudioClip boomClip;
    public AudioClip[] hitClips;
    public GameObject hitEffect;

    [Header("Shooting")]
    public GameObject enemyBullet;
    public float currentDelay;
    public float fireRate;
    public Transform spawnPoint;
    Transform player;
    public bool canShoot;

    [Header("HitPoints and props")]
    public int hitPoints;
    int currentHP;
    public bool isHealthbar;
    public Slider healBar;
    public TrailRenderer[] trailRender;

    [Header("Waves settings")]
    public int currentWaypointID = 0;
    [Range(1,20)]
    public float speed = 5;
    public float diveSpeed = 5;
    public float reachDistance = 0.4f;
    public float rotationSpeed = 5f;    
    float currentDistance;//to next waypoint
    public bool useBezier;
    public bool useRotate;

    [Header("Scoring")]
    public int inFormationScore;
    public int outFormationScore;
    public GameObject pickUP;
    public GameObject pickDOWN;

    bool canSpawn;


    //state machine for movement randoms flyout from formation
    
    public enum EnemyStates
    {
        ON_PATH,  //FLYING ON PATH
        FLY_IN, //BACK TO FORMATION
        IDLE,
        DIVE
    }

    [Header("Paths")]
    public EnemyStates enemyStates;

    public int enemyID;
    public Formation formation;


    public void Awake()
    {
        if (instance is null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHP = hitPoints;
        if (!Classic_PlayerBehaviour.instance.hasLost)
        {
            player = GameObject.Find("PlayerShip").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyStates)
        {
            case EnemyStates.ON_PATH:
                {
                    TrailActive(true);
                    MoveOnThePath(pathToFollow);
                    canSpawn = false;
                }
                break;

            case EnemyStates.FLY_IN:
                {
                    canSpawn = true;
                    MoveToFormation();
                }

                break;

            case EnemyStates.IDLE:
                {
                    canSpawn = true;
                    TrailActive(false);
                }

                break;
            case EnemyStates.DIVE:
                {
                    speed = diveSpeed;
                    TrailActive(true);
                    canSpawn = true;
                    MoveOnThePath(pathToFollow);

                    //shooting
                    if (canShoot)
                    {
                        SpawnBullet();
                    }
                }

                break;
        }

        //healthbar
        if (isHealthbar)
        {
            healBar.value = currentHP;
        }


        //check HP
        if(currentHP <= 0)
        {
            
            Invoke("EnemyDestroyed", 0);
        }
    }

    private void MoveToFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, formation.GetVector(enemyID), speed * Time.deltaTime);


        //rotation while flying in to formation
        // look AT direction where fly
        var direction = formation.GetVector(enemyID) - transform.position;
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            direction = direction.normalized;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }


        if (Vector3.Distance(transform.position, formation.GetVector(enemyID)) <= 0.0001f)
        {
            transform.SetParent(formation.gameObject.transform);
            transform.eulerAngles = new Vector3(0,180,0);

            formation.enemyList.Add(new Formation.EnemyFormation(enemyID, transform.localPosition.x, transform.localPosition.z, this.gameObject));

            enemyStates = EnemyStates.IDLE;
            
        }
    }

    void MoveOnThePath(Path path)
    {
        if (useBezier)
        {
            currentDistance = Vector3.Distance(path.bezierObjList[currentWaypointID], transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.bezierObjList[currentWaypointID], speed * Time.deltaTime);

            if (useRotate)
            {
                // look AT direction where fly
                var direction = path.bezierObjList[currentWaypointID] - transform.position;
                if (direction != Vector3.zero)
                {
                    direction.y = 0;
                    direction = direction.normalized;
                    var rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            currentDistance = Vector3.Distance(path.pathObjList[currentWaypointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.pathObjList[currentWaypointID].position, speed * Time.deltaTime);

            if (useRotate)
            {
                // look AT direction where fly
                var direction = path.pathObjList[currentWaypointID].position - transform.position;
                if (direction != Vector3.zero)
                {
                    direction.y = 0;
                    direction = direction.normalized;
                    var rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
          
        if (useBezier)
        {
            // if reaches point ONE, move towards next? (++) or change to else??
            if (currentDistance <= reachDistance)
            {
                //transform.LookAt(path.pathObjList[currentWaypointID++]);
                currentWaypointID++;
            }
            //when reaches point LAST, repeat(by def)
            if (currentWaypointID >= path.bezierObjList.Count)
            {
                

                //make loop, set null to stay at last                
                currentWaypointID = 0;
                enemyStates = EnemyStates.FLY_IN;
            }
        }
        else
        {
            // if reaches point ONE, move towards next? (++) or change to else??
            if (currentDistance <= reachDistance)
            {
                //transform.LookAt(path.pathObjList[currentWaypointID++]);
                currentWaypointID++;
            }
            //when reaches point LAST, repeat(by def)
            if (currentWaypointID >= path.pathObjList.Count)
            {
                //print("last point of way");
                
                //make loop, set null to stay at last                
                currentWaypointID = 0;
                enemyStates = EnemyStates.FLY_IN;
            }
        }
    }

    public void SpawnSetup(Path path, int ID, Formation _formation)
    {
        pathToFollow = path;
        enemyID = ID;
        formation = _formation;
    }

    public void DiveSetup(Path path)
    {
        pathToFollow = path;
        transform.SetParent(transform.parent.parent);
        enemyStates = EnemyStates.DIVE;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag is "Laser")
        {

            RFX4_CameraShake.Instance.PlayShake();
            currentHP--;
            Instantiate(hitEffect,transform.position,transform.rotation);
            int hitClip = Random.Range(0, hitClips.Length);
            if (GameManager.instance.audioEnabled)
            {
                audioSource.PlayOneShot(hitClips[hitClip]);
            }
            
        }

        if(other.tag == "Ball"){
            
            currentHP--;
            Instantiate(hitEffect,transform.position,transform.rotation);
            int hitClip = Random.Range(0, hitClips.Length);
            if (GameManager.instance.audioEnabled)
            {
                audioSource.PlayOneShot(hitClips[hitClip]);
            }
        }

        if(other.tag is "Player")
        {
            
            GameManager.instance.TakeColision();
            currentHP--;
            Classic_PlayerBehaviour.instance.TakeDamage(-1);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag is "Laser")
        {
            
            int hitClip = Random.Range(0, hitClips.Length);
            currentHP--;
            Instantiate(hitEffect,transform.position,transform.rotation);
            if (GameManager.instance.audioEnabled)
            {
                audioSource.PlayOneShot(hitClips[hitClip]);
            }
        }

        if(other.gameObject.tag is "Player")
        {
            GameManager.instance.TakeColision();
            currentHP--;
            Classic_PlayerBehaviour.instance.TakeDamage(-1);
        }
    }

    public void SpawnBullet()
    {
        currentDelay += Time.deltaTime;
        if(currentDelay >= fireRate && enemyBullet != null && spawnPoint != null)
        {
            spawnPoint.LookAt(player);
            Instantiate(enemyBullet, transform.position, spawnPoint.rotation);
            currentDelay = 0;
        }
    }

    public void ExplosionEffects() {
        int explosionsEffect = Random.Range(0,explosionsEffects.Length);
        Instantiate(explosionsEffects[explosionsEffect], transform.position, transform.rotation);
    }
    public void EnemyDestroyed()
    {

        
        if (enemyStates == EnemyStates.IDLE)
        {
            GameManager.instance.AddScore(inFormationScore);
        }
        else
        {
            GameManager.instance.AddScore(outFormationScore);
        }
//        onDeathEvent.Invoke();
        ExplosionEffects();
        //Instantiate(explosionsEffect, transform.position, transform.rotation);
        //Instantiate(boomEfect, transform.position, transform.rotation);
        //GameManager.instance.Shake();
        //if (GameManager.instance.audioEnabled)
        //{
        //    GameManager.instance.EnemyExplosion();
        //}

        //report to formation object is destroyed
        for (int i = 0; i < formation.enemyList.Count; i++)
        {
            if (formation.enemyList[i].index == enemyID)
            {
                formation.enemyList.Remove(formation.enemyList[i]);
            }
        }

        
        SpawnManager sp = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //send to spawn manager destroing of each
        for (int i = 0; i < sp.spawnedEnemies.Count; i++)
        {
            sp.spawnedEnemies.Remove(this.gameObject);
        }

        //send to gamemanager you killed a ship

        GameManager.instance.RemoveEnemy();
        
        int randomNumber = Random.Range(0, 100);
        if(canSpawn){
        if (randomNumber < 10)
        {
            Instantiate(pickUP, transform.position, transform.rotation);
        }
        
            
        

        else if (randomNumber > 90)
        {
            Instantiate(pickDOWN, transform.position, transform.rotation);
        }
        }
        Classic_PlayerBehaviour.instance.CallEvent();

        Destroy(gameObject);
        //ExploderSingleton.ExploderInstance.ExplodeObject(gameObject);
    }

    public void TrailActive(bool on)
    {
        foreach(TrailRenderer trail in trailRender)
        {
            trail.enabled = on;
        }
    }

    
}
