using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public MapLimits Limits;
    public GameObject[] enemys;
    public float spawnTimer;
    public float maxSpawnTimer;
    public GameObject pausePanel;
    public GameObject inGameOptions;

    // Use this for initialization
    void Start() {
        spawnEnemy();
        maxSpawnTimer = spawnTimer;
        //DontDestroyOnLoad(pausePanel);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnEnemy();
            spawnTimer = maxSpawnTimer;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            pausePanel.SetActive(true);
            pausePanel.GetComponent<Animator>().Play("FadeIn");
            Time.timeScale = 0f;
        }
    }

    void spawnEnemy()
    {

        int selEnemy = Random.Range(0, enemys.Length);
        Instantiate(enemys[selEnemy], new Vector3(Random.Range(Limits.minimumX, Limits.maximumX) + -5,
        Random.Range(Limits.minimumY, Limits.maximumY), 0), Quaternion.Euler(90, 0, 0));
                  
                       
    }

    public void playAgain()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Resume()
    {
        
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void timeSc()
    {
        Time.timeScale = 1f;
    }

    public void timeS()
    {
        pausePanel.SetActive(true);
        pausePanel.GetComponent<Animator>().Play("FadeIn");

        Time.timeScale = 0f;
    }

    public void Options()
    {

    }

    public void exitMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void fadeInOptions()
    {
        pausePanel.SetActive(false);
        inGameOptions.GetComponent<Animator>().Play("OptionsInGame");
    }
}
