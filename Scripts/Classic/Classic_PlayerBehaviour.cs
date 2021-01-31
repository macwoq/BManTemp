using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class Classic_PlayerBehaviour : MonoBehaviour
{
    public static Classic_PlayerBehaviour instance;

    [Header("Shooting")]
    public UnityEvent unityEvent;
    public GameObject _laser;
    public GameObject _muzzle;
    public Transform _mainSpawn;
    public Transform _leftSpawn;
    public Transform _rightSpawn;
    public bool canShoot = true;
    bool isJoy;

    [Header("Shooting Rate")]
    public float _speed;
    public float _fireRate = 0.1f;
    public float _nextFire = 0f;
    public float gyroSpeed;

    [Header("Pickups and Health")]
    public int bulletLevel = 1;

    public int playerHealth = 3;
    [Range(1,3)]
    public int currentHealth;
    public bool hasLost;
    public GameObject playerExplosion;
    public GameObject noHoover;

    [Header("Sounds")]
    public AudioSource audioS;
    public AudioClip _fire;
    public AudioClip _fire2;
    public AudioClip _fire3;

    [Header("Playground Bounds")]
    //public AudioClip _shield;
    //public AudioClip _tripleShot;
    //public AudioClip _laserBeam;
    public float xBoundsL = -11f, xBoundsR = 11;
    public float zBounds = 5.5f, zBoundUp = -1.5f;
    Animator anim;

    [Header("Reset Player After Loosing")]
    //public TextMeshProUGUI counter;
    public float startCounter = 10;

    Vector3 initialPosition;

    public void Awake()
    {
        instance = this;

        

    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.goText.SetActive(false);
        //CallEvent();
        audioS = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        if (PlayerPrefs.GetInt("setHealth") == 1)
        {
            currentHealth = 1;
        }

        if (PlayerPrefs.GetInt("setHealth") == 2)
        {
            currentHealth = 2;
        }

        if (PlayerPrefs.GetInt("setHealth") == 3)
        {
            currentHealth = 3;
        }
        //bulletLevel = 1;
        bulletLevel = PlayerPrefs.GetInt("bullet", 1);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame(){
        //GameManager.instance.goText.SetActive(false);
        
        playerHealth = currentHealth;
        GameManager.instance.coolDownText.SetActive(true);
        canShoot = false;
        yield return new WaitForSeconds(startCounter);
        GameManager.instance.coolDownText.SetActive(false);
        canShoot = true;
        GameManager.instance.goText.SetActive(true);
        yield return new WaitForSeconds(1);
        GameManager.instance.goText.SetActive(false);
    }

    private void Update()
    {



        //if (currentHealth == 1)
        //{
        //    playerHealth = 1;
        //} else 
        //if (currentHealth == 2)
        //{
        //    playerHealth = 2;
        //} else
        //if (currentHealth == 3)
        //{
        //    playerHealth = 3;
        //}

        //Movement();
        if (PlayerPrefs.GetInt("joyType") == 0)
        {
            //isJoy = true;
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector3(-horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);


        }

        if (PlayerPrefs.GetInt("joyType") == 1)
        {
            //isJoy = true;
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector3(-horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);


        }

        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //startCounter -=Time.time * Time.deltaTime;
        //counter.text = startCounter.ToString();
        //Movement();
        Shooting();
        Bounds();
    }

    

    public void ShootInput()
    {
        if (canShoot && !hasLost)
        {
            _nextFire = Time.time + _fireRate;

            switch (bulletLevel)
            {
                case 1:
                    {
                        Instantiate(_muzzle, _mainSpawn.position, transform.rotation);
                        Instantiate(_laser, _mainSpawn.position, transform.rotation);

                        if (GameManager.instance.audioEnabled)
                        {
                            audioS.PlayOneShot(_fire);
                        }

                    }
                    break;

                case 2:
                    {
                        Instantiate(_muzzle, _leftSpawn.position, transform.rotation);
                        Instantiate(_laser, _leftSpawn.position, transform.rotation);

                        Instantiate(_muzzle, _rightSpawn.position, transform.rotation);
                        Instantiate(_laser, _rightSpawn.position, transform.rotation);

                        if (GameManager.instance.audioEnabled)
                        {
                            audioS.PlayOneShot(_fire2);
                        }

                    }
                    break;

                case 3:
                    {
                        Instantiate(_muzzle, _mainSpawn.position, transform.rotation);
                        Instantiate(_laser, _mainSpawn.position, transform.rotation);


                        Instantiate(_muzzle, _leftSpawn.position, transform.rotation);
                        Instantiate(_laser, _leftSpawn.position, transform.rotation);

                        Instantiate(_muzzle, _rightSpawn.position, transform.rotation);
                        Instantiate(_laser, _rightSpawn.position, transform.rotation);

                        if (GameManager.instance.audioEnabled)
                        {
                            audioS.PlayOneShot(_fire3);
                        }

                    }
                    break;
            }
        }                                                                                                                                                                                                                                                                   
    }

    public void Shooting()
    {
        if(canShoot)
        {
            if (!hasLost)
            {
                if (Input.GetButton("Jump") && Time.time > _nextFire)
            {

                _nextFire = Time.time + _fireRate;

                switch (bulletLevel)
                {
                    case 1:
                        {
                            Instantiate(_muzzle, _mainSpawn.position, transform.rotation);
                            Instantiate(_laser, _mainSpawn.position, transform.rotation);

                            if (GameManager.instance.audioEnabled)
                            {
                                audioS.PlayOneShot(_fire);
                            }
                            
                        }
                        break;

                    case 2:
                        {
                            Instantiate(_muzzle, _leftSpawn.position, transform.rotation);
                            Instantiate(_laser, _leftSpawn.position, transform.rotation);

                            Instantiate(_muzzle, _rightSpawn.position, transform.rotation);
                            Instantiate(_laser, _rightSpawn.position, transform.rotation);

                            if (GameManager.instance.audioEnabled)
                            {
                                audioS.PlayOneShot(_fire2);
                            }
                            
                        }
                        break;

                    case 3:
                        {
                            Instantiate(_muzzle, _mainSpawn.position, transform.rotation);
                            Instantiate(_laser, _mainSpawn.position, transform.rotation);


                            Instantiate(_muzzle, _leftSpawn.position, transform.rotation);
                            Instantiate(_laser, _leftSpawn.position, transform.rotation);

                            Instantiate(_muzzle, _rightSpawn.position, transform.rotation);
                            Instantiate(_laser, _rightSpawn.position, transform.rotation);

                            if (GameManager.instance.audioEnabled)
                            {
                                audioS.PlayOneShot(_fire3);
                            }

                        }
                        break;
                }









            }
            }
        }

    }


    public void CallEvent(){
        unityEvent.Invoke();
    }

    private void Bounds()
    {
        if(transform.position.z <= -1.5)
        {
            transform.position = new Vector3(transform.position.x, 0, zBoundUp);
        }else 
        
            if (transform.position.z >= 5.5)
        {
            transform.position = new Vector3(transform.position.x, 0, zBounds);
        }

            if(transform.position.x < xBoundsL)
        {
            transform.position = new Vector3(xBoundsR, transform.position.y, transform.position.z);
        }else
            if(transform.position.x > xBoundsR)
        {
            transform.position = new Vector3(xBoundsL, transform.position.y, transform.position.z);
        }



    }

    private void Movement()
    {

        if (!hasLost)//&&!isJoy)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector3(-horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

            if(horizontalInput == 1)
            {
                //anim.Play("BankingL");
            }else if (horizontalInput == -1)
            {
                //anim.Play("BankingRight");
            }
        }
    }


    public void TakeDamage(int damage)
    {
        //SoundsManager.Instance.PlayEffect("");
        currentHealth+= damage;

        if(currentHealth <= 0)
        {
            
            GameManager.instance.PlayerExplode();
            //SoundsManager.instance.PlayEffect("laser");
            Instantiate(playerExplosion, transform.position, transform.rotation);
            //particles
            //Destroy(gameObject);

            StartCoroutine(ResetGame());
        }
    }

    IEnumerator ResetGame()
    {
        GameManager.instance.DecreaseLifes();
        noHoover.SetActive(false);
        //GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        hasLost = true;
        PlayerPrefs.DeleteKey("bullet");
        transform.position = initialPosition;

        yield return new WaitForSeconds(1f);
        noHoover.SetActive(true);
        Shield.instance.StartShield();
        //GetComponent<MeshRenderer>().enabled = true;
        bulletLevel = 1;
        //currentHealth = PlayerPrefs.GetInt("getDiff");
        currentHealth = playerHealth;
        hasLost = false;
        GameManager.instance.anima.SetActive(false);

        yield return new WaitForSeconds(2);
        GetComponent<Collider>().enabled = true;
    }
}
