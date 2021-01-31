using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;

using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    

    private const int V = 1;
    public static GameManager instance;

    public AudioSource audioSource;
    public AudioSource musicSource;


    [Header("Game Sounds")]
    public bool audioEnabled = true;
    public bool musicEnabled = true;

    public AudioMixer soundMixer;
    public AudioMixer musicMixer;

    public int audioOn = 1;
    public int musicOn = 1;
    public GameObject pausePanel;
    //public Slider soundSlider;
    //public Slider musicSlider;

    public AudioClip[] levelMusics;
    //public AudioClip enemyExplo;
    public AudioClip playerExplode;
    public AudioClip[] enemyExplode;
    public AudioClip pickUP;
    public AudioClip pickDOWN;
    public AudioClip noPICK;
    public AudioClip stageUp;
    public AudioClip lifeDown;
    public AudioClip getHit;
    public AudioClip takeColl;

    [Header("Score & Lifes")]
    public UnityEvent endLevel;
    public GameObject gameOverPanel;
    static public int level = 1;
    static public int score =0;
    static public int lifes = 3;//not HP!!!
    public int currentLifes;
    public float timing = 5;
    public int currentLevel;    
    public int enemyAmmount;

    public int scoreToBonusLife = 1000;

    public int bonusScore;
    public bool isPaused = false;
    static bool isLost;
    public int topScore;
    public GameObject stageComplete;
    public int maxLevel;
    public int blinkTime = 0;
    public int blinkT = 5;
    public GameObject coolDownText;
    public GameObject goText;
    public UnityEvent goPro;
    public GameObject levelCompletePanel;
    public string sceneName;

    

    public int levelNumber;
    public int levelDone;
    public int sceneIndex;
    

    public bool isEndless = false;

    /*
    ship one = 50, flying 100
    ship two = 80, 160
    three = 100, 400
    */

 

    [Header("Anim&Props")]
    Animator anim;
    public GameObject anima;
    //public Animation lifeDownAnim;
    
    public void Awake()
    {
        //if(instance != null){
        //    Destroy(this.gameObject);
        //    
        //}
        //else
        //{
            instance = this;
        //}
         //   DontDestroyOnLoad(this.gameObject);
      

        //MainMenu.instance.ShowStageText();
        

        if (isLost)
        {
            level = 1;
            score = 0;
            lifes = 3;
            //bonusScore = 0;
            isLost = false;
        }
    }

    private void OnDestroy() {

        if(instance == this){
            instance = null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        PlayerPrefs.GetInt("audioOn");
        if(audioOn == 1){audioEnabled = true;}else{audioEnabled = false;}
        PlayerPrefs.GetInt("musicOn");
        if(musicOn == 1){musicEnabled = true;}else{musicEnabled = false;}
        
        if(PlayerPrefs.GetInt("exitGame") == 1){
        level = 1;
        score = 0;
        lifes = 3;
        //bonusScore = 0;
        Classic_PlayerBehaviour.instance.bulletLevel = 1;
        PlayerPrefs.SetInt("exitGame", 0);
        }
            else
        {
            Debug.Log("null!");
        }
        enemyAmmount = 0;
        MainMenu.instance.UpdateScoreText(score);
        MainMenu.instance.UpdateLifeText(lifes);
        isPaused = false;
        if (isEndless)
        {
            MainMenu.instance.ShowStageText(level);
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!audioEnabled)
        {
            soundMixer.SetFloat("Sound", Mathf.Log10(0.0001f) * 20);
            PlayerPrefs.SetInt("audioOn", 0);
        }else if(audioEnabled)
        {
            soundMixer.SetFloat("Sound", Mathf.Log10(1f) * 20);
            PlayerPrefs.SetInt("audioOn", 1);
        }

        else if (!musicEnabled)
        {
            musicMixer.SetFloat("Music", Mathf.Log10(0.0001f) * 20);
            PlayerPrefs.SetInt("musicOn", 0);
        }else if (musicEnabled)
        {
            musicMixer.SetFloat("Music", Mathf.Log10(1f) * 20);
            PlayerPrefs.SetInt("musicOn", 1);
        }

        topScore = PlayerPrefs.GetInt("topScore");
        
        if (Input.GetButtonDown("Cancel"))
        {
            PauseUnpause();
        }
        if (isPaused)
        {
            Time.timeScale = 0;
        }else
        {
            Time.timeScale = 1;
        }

        //if(level == 2)
        //{
        //    ExitFromGame();
        //}
        //GetLevel();
        //lifes = currentLifes;
    }

    
        
    
    public void PauseUnpause()
    {
        if(pausePanel != null){
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            isPaused = true;
        }
        else
        {
            pausePanel.SetActive(false);
            isPaused = false;
        }
        }

        
        
    }

    public void EnemyExplosion()
    {
        int enemyExplo = Random.Range(0, enemyExplode.Length);
        
        if (audioEnabled)
        if(audioSource != null)
        audioSource.PlayOneShot(enemyExplode[enemyExplo]);
        
        
    }

    public void PlayerExplode()
    {
        if (audioEnabled)
        {
            audioSource.PlayOneShot(playerExplode);
        }
    }
    public void TakeColision()
    {
        if (audioEnabled)
        {
            audioSource.PlayOneShot(takeColl);
        }
    }

    public void GetHit()
    {
        if (audioEnabled)
        {
            audioSource.PlayOneShot(getHit);
        }
    }

    public void PlayLevelMusic()
    {
        int levelMusic = Random.Range(0, levelMusics.Length);
        if (musicEnabled)
        {
            
            musicSource.PlayOneShot(levelMusics[levelMusic]);
        }
        //musicSource.clip = levelMusic
    }


    public void AudioMixerDown(float sound)
    {
        
        //soundMixer.SetFloat("Sound", Mathf.Log10(sound) * 20);
        soundMixer.SetFloat("Sound", Mathf.Log10(sound) * 20);
        
    }


    public void ToggleAudio()
    {
        audioEnabled = !audioEnabled;
    }

    public void MusicToggle(){
        musicEnabled = !musicEnabled;
    }

    public void MusicEnabled()
    {
        musicEnabled = true;
    }

    public void MusicDisabled(){
        musicEnabled = false;
    }
    
    public void PickUp(int type)
    {
        if (audioEnabled)
        {
            if(type == 1)
            {
                //Classic_PlayerBehaviour.instance.bulletLevel++;
                //if(Classic_PlayerBehaviour.instance.bulletLevel == 2)
                //{
                    //audioSource.PlayOneShot(noPICK);
                //}
               //else
               //{
                    audioSource.PlayOneShot(pickUP);
                //}

            }
            if(type == 0)
            {
                //Classic_PlayerBehaviour.instance.bulletLevel--;
                //if(Classic_PlayerBehaviour.instance.bulletLevel == 0)
                //{
                //    audioSource.PlayOneShot(noPICK);
                //}else
                //{
                    audioSource.PlayOneShot(pickDOWN);
                //}
            }
            if(type == 2)
            {
                audioSource.PlayOneShot(noPICK);
            }

        }
    }

    public void AddScore(int ammount)
    {
        score += ammount;

        bonusScore += ammount;

        MainMenu.instance.UpdateScoreText(score);

        MainMenu.instance.UpdateBonusScore(bonusScore);

        if(bonusScore >= scoreToBonusLife)
        {
            lifes++;
            MainMenu.instance.UpdateLifeText(lifes);
            bonusScore %= scoreToBonusLife;
        }

        PlayerPrefs.SetInt("score", score);
        if(PlayerPrefs.GetInt("topScore")<= score)
        {
            PlayerPrefs.SetInt("topScore", score);
        }
    }

    public void DecreaseLifes()
    {
        lifes--;
        if (audioEnabled)
        {
            audioSource.PlayOneShot(lifeDown);
        }
        //
        //isLost = true;
        anima.SetActive(true);
        anim.Play("LifeDown");
        MainMenu.instance.UpdateLifeText(lifes);
        if (lifes < 0)
        {
            Time.timeScale = 0;
            //Invoke("EndLevel",1);
            endLevel.Invoke();
        }
    }

    void Blink()
    {
        if(blinkTime < blinkT)
        {
            blinkTime++;
            stageComplete.SetActive(true);
            Invoke("NextBliink", 0.5f);
            
        }   else
        {
            stageComplete.SetActive(false);
        }

        
    }

    void NextBliink()
    {
        stageComplete.SetActive(false);
        Invoke("Blink", 0.5f);
    }

    public void AddEnemy()
    {
        enemyAmmount += 1;
    }

    public void RemoveEnemy()
    {
        enemyAmmount--;
        
        EnemyExplosion();
        if(enemyAmmount <= 0)
        {
            blinkTime = 0;
            Blink();
            //check win condition
            //
            if (!isEndless)
            {
                Invoke("StageComplete", 3);
            }
            else if (isEndless)
            {
                Invoke("NextStage", 3);
            }
        }
    }

    public void NextStage()
    {
        
        level++;
        if (audioEnabled)
        {
            audioSource.PlayOneShot(stageUp);
        }
        //PlayerPrefs.SetInt("level", level);
        //if (PlayerPrefs.GetInt("maxLevel") <= level){
        //    PlayerPrefs.SetInt("maxLevel", level);
        //}
        MainMenu.instance.ShowStageText(level);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitFromGame(){
        audioSource.Stop();
        musicSource.Stop();
        PlayerPrefs.SetInt("exitGame", 1);
        SceneManager.LoadScene("MainMenu");
    }
    public void ReloadScene(){
        audioSource.Stop();
        musicSource.Stop();
        //yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("exitGame", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverPanel.SetActive(false);
    }

    public void SetVibration(int motorIndex, float motorLevel)
    {
        
    }

    public void StageComplete()
    {
        goPro.Invoke();
        SaveLoadLevels(levelNumber);
        //1levelCompletePanel.SetActive(true);
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SaveLoadLevels(int levelNumber)
    {
        if(levelNumber == 1)
        {
            
            PlayerPrefs.SetInt("lastLevel", 1);
            
        }

        if(levelNumber == 2)
        {
            
            PlayerPrefs.SetInt("lastLevel", 2);
            
        }

        if (levelNumber == 3)
        {
            
            PlayerPrefs.SetInt("lastLevel", 3);
            
        }

        if (levelNumber == 4)
        {
            
            PlayerPrefs.SetInt("lastLevel", 4);
            
        }

        if (levelNumber == 5)
        {
            
            PlayerPrefs.SetInt("lastLevel", 5);
            
        }

        if (levelNumber == 6)
        {
            
            PlayerPrefs.SetInt("lastLevel", 6);
            
        }
    }

    public void GetLevel()
    {
        
        if (PlayerPrefs.GetInt("lastLevel") == 1)
        {
            print("1");
        }

        if (PlayerPrefs.GetInt("lastLevel") == 2)
        {
            print("2");
        }

        if (PlayerPrefs.GetInt("lastLevel") == 3)
        {
            print("3");
        }

        if (PlayerPrefs.GetInt("lastLevel") == 4)
        {

        }

        if (PlayerPrefs.GetInt("lastLevel") == 5)
        {

        }
    }
     


}
