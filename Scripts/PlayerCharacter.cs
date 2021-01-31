using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter instance;

    public float movementSpeed;
    public float isMoving;
    private Rigidbody rb;
    public MapLimits Limits;
    public float shotPower;
    public GameObject bullet;
    public Transform pos1;
    public Transform posL;
    public Transform posR;
    public AudioSource audioS;
    public AudioSource PlayerS;
    public AudioClip powUp;
    public AudioClip powDown;
    public AudioClip shotSound;    
    public AudioClip playerExpl;
    public AudioClip playerHit;
    public AudioClip explosion;
    public int power;
    public GameObject particle;
    public GameObject playerDeathP;
    public int hP;
    public int score;
    int highScore;
    public Text scoreText;
    public Text highScoreText;
    public Text highScoreText2;
    public Text hpText;
    public GameObject pausePanel;
    public GameObject HP1;
    public GameObject HP2;
    public GameObject HP3;
    
    
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //power = 1;
        audioS = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.GetInt("highscore", highScore);
            PlayerPrefs.SetInt("highscore", highScore);
        }
        score = 0;
        highScore = PlayerPrefs.GetInt("highscore", highScore);
        hP = 3;
        HP1.SetActive(true);
        HP2.SetActive(true);
        HP3.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        highScoreText2.text = PlayerPrefs.GetInt("highscore").ToString();
        Movement();
        Shooting();
        hpText.text = hP.ToString();

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
            
        }

        if (hP <= 0)
        {
            
            playerExplode();
        }
        
        activeBounds();
        hitPoints();

        
    }

    

    private void activeBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, Limits.minimumX, Limits.maximumX),
            Mathf.Clamp(transform.position.y, Limits.minimumY, Limits.maximumY), 0.0f);
    }

    public void Movement()
    {
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(Vector3.right * -movementSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        //
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(Vector3.up * -movementSpeed * Time.deltaTime);
        //}
        //Time.timeScale = 1f;
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        rb.AddForce(movement * movementSpeed);

        if (Input.GetAxis("Horizontal") == 1)
        {
            
            GetComponent<Animator>().Play("right_rotate", -1, 0f);
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
            GetComponent<Animator>().Play("left_rotate", -1, 0f);
            
        }



    }

    public void Shooting()
    {
        //Time.timeScale = 1f;
        if (Input.GetButtonDown("Jump"))
        {
            Time.timeScale = 1f;
            audioS.PlayOneShot(shotSound);
            switch (power)
            {
                case 1:
                    {
                        //Instantiate(particle, transform.position, transform.rotation);
                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    }
                    break;
                case 2:
                    {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;

                    }
                    break;
                case 3:
                    {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet3 = Instantiate(bullet, pos1.position, transform.rotation);
                        bullet3.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;



                    }
                    break;
                default:
                    {
                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    }
                    break;
            }

        }
        
            
        
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="powerUp")
        {
            if (power < 3)
                power++;
            audioS.PlayOneShot(powUp);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "powerDown")
        {
            if (power > 1)
                power--;
            audioS.PlayOneShot(powDown);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "enemyBullet")
        {
            
                hP--;
            

            audioS.PlayOneShot(playerHit);
            Destroy(col.gameObject);
        }
    }

    public void hitPoints()
    {
        if (hP == 2)
        {
            HP3.SetActive(false);
        }
        if (hP == 1)
        {
            HP2.SetActive(false);
        }
        if(hP == 0)
        {
            HP1.SetActive(false);
        }
    }

    void playerExplode()
    {
        pausePanel.SetActive(true);
        pausePanel.GetComponent<Animator>().Play("FadeIn");
        GameObject obj = GameObject.Find("ExplodeAudio");
        AudioSource aud = obj.GetComponent<AudioSource>();
        aud.PlayOneShot(explosion);
        Destroy(gameObject);
        Instantiate(playerDeathP, transform.position, transform.rotation);
        
    }

    public void playerShoot()
    {


        audioS.PlayOneShot(shotSound);

        switch (power)

        {

            case 1:
                {
                    Instantiate(particle, transform.position, transform.rotation);
                    GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                    newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                }
                break;

            case 2:
                {
                    GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                    bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                    bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;

                }
                break;

            case 3:
                {
                    GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                    bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                    bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    GameObject bullet3 = Instantiate(bullet, pos1.position, transform.rotation);
                    bullet3.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;



                }
                break;

            default:
                {
                    GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                    newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                }
                break;
        }

    }

    public void pausePanelActive()
    {
        pausePanel.SetActive(true);
    }

    public void incrementScore()
    {
        score += 1;
    }

    public void startScore()
    {
        InvokeRepeating("incrementScore", 0.1f, 0.5f);
    }

    public void stopScore()
    {
        //CancelInvoke("startScore");
        PlayerPrefs.SetInt("score", score);

        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", score);
            }
        } else
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }


}
