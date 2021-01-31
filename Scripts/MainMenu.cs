using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.IO;
using System;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;

    public string privacyPolicyLink;

    [Header("Scores")]
    public int level;
    public int maxLevel;
    public TextMeshProUGUI maxStage;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScore;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI topScore;
    public TextMeshProUGUI proScore;

    [Header("Sound")]
    public GameObject soundPanel;
    public AudioMixer mmixer;
    public AudioMixer smixer;
    public Slider sfxSlider;
    public Slider mscSlider;
    public Slider joyType;
    public GameObject dPad;
    public GameObject joy;
    public bool useGyro = false;

    [Header("Quality")]
    [SerializeField]
    private int qLevel;
    public int qualityL;
    public float q1,q2,q3; 

    [Header("Levels")]
    [SerializeField]
    private int levelEasy;
    private int levelMed;
    private int levelHard;
    public string tempScene;
    public string trainingScene;
    [Range(1,3)]
    public int setHealth;
    [SerializeField] Slider setH;
    //[SerializeField] Slider setHealthValue;

    public void Awake()
    {
        //if(instance is null)
        instance = this;
        
    }

    private void OnDestroy()
    {
        if(instance is null)
        {
            instance = this;
        }
    }

    void Start () {
        //PlayLogin.instance.SignIn();
        qLevel = QualitySettings.GetQualityLevel();
        Time.timeScale = 1f;
        //level = PlayerPrefs.GetInt("level", 1);
        //maxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        if (joyType != null)
        {
            joyType.value = PlayerPrefs.GetInt("joyType", 0);
        }
        //topScore.text = PlayerPrefs.GetInt("highscore").ToString();
        if (mscSlider != null)
        {
            mscSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        }
        if (topScore != null)
        {
            topScore.text = PlayerPrefs.GetInt("topScore").ToString();
        }
        //if (stageText != null)
        //{
        //    maxStage.text = PlayerPrefs.GetInt("maxLevel").ToString();
        //}
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (PlayerPrefs.GetInt("setHealth") == 1)
            {
                setH.value = 1;
            }

            if (PlayerPrefs.GetInt("setHealth") == 2)
            {
                setH.value = 2;
            }

            if (PlayerPrefs.GetInt("setHealth") == 3)
            {
                setH.value = 3;
            }
        }

        if (PlayerPrefs.GetInt("setHealth") == 1)
        {
            setHealth = 1;
        }

        if (PlayerPrefs.GetInt("setHealth") == 2)
        {
            setHealth = 2;
        }

        if (PlayerPrefs.GetInt("setHealth") == 3)
        {
            setHealth = 3;
        }

    }

    private void Update()
    {
        JoyType();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SetHealth();
        }
    }

    private void SetHealth()
    {
        
        
        if (setH.value == 1)
        {
            PlayerPrefs.SetInt("setHealth", 1);
        }
        if (setH.value == 2)
        {
            PlayerPrefs.SetInt("setHealth", 2);
        }
        if (setH.value == 3)
        {
            PlayerPrefs.SetInt("setHealth", 3);
        }
        
    }

    public void UpdateBonusScore(int ammount)
    {
        bonusText.text = ammount.ToString("D5");
    }
	
	public void UpdateScoreText(int ammount)
    {
        scoreText.text = ammount.ToString("D9");
        endScore.text = ammount.ToString("D8");
        //if (!GameManager.instance.isEndless)
        //{
        //    //proScore.text = ammount.ToString("D8");
        //}
    }
    public void UpdateLifeText(int ammount)
    {
        lifeText.text = "x" + ammount.ToString("D2");
    }
    public void ShowStageText(int ammount)
    {
        stageText.gameObject.SetActive(true);
        stageText.text = "Stage " + ammount;
        Invoke("DeactivateStageText", 3f);
    }

    void DeactivateStageText()
    {
        stageText.gameObject.SetActive(false);
        //stageText.text = "Stage " + ammount;
        CancelInvoke("DeactivateStageText");
    }

    public void SetQ(int quality){
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("quality", quality);
        qualityL = quality;
    }
    public void GetQ(){
        PlayerPrefs.GetInt("quality", 2);
        
    }

    public void EraseData(){
        PlayerPrefs.DeleteAll();
    }


    public void calssicMode()
    {
        SceneManager.LoadScene("Classic");
    }

    public void arcadeMode()
    {
        //GameManager.instance.ReloadScene();
        SceneManager.LoadScene("GameplayEasy");
    }

    public void arcadeModeEasy()
    {
        //GameManager.instance.ReloadScene();
        SceneManager.LoadScene("Gameplay");
    }

    public void Options()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    
    
    public void soundOn()
    {

    }

    public void soundOff()
    {

    }

    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(){
        GameManager.instance.currentLevel =1;
        SceneManager.LoadScene("GameOver");
    }
    public void mainScreen()
    {
        
        SceneManager.LoadScene("MainMenu");
    }

    public void classicMode()
    {
        SceneManager.LoadScene("Classic");
        
    }

    public void soundPanelIn()
    {
        soundPanel.GetComponent<Animator>().Play("VolPanel");
    }

    public void soundPanelOut()
    {
        soundPanel.GetComponent<Animator>().Play("VolPanelOut");
    }

    public void SetMusic(float sliderValue)
    {
        mmixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void setSfxMin(float sliderValue)
    {
        smixer.SetFloat("Sound", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

    

    public void JoyType()
    {
        if (joyType != null)
        {
            if (joyType.value == 1)
            {
                dPad.SetActive(false);
                joy.SetActive(true);
                PlayerPrefs.SetInt("joyType", 1);
            }
            if (joyType.value == 0)
            {
                dPad.SetActive(false);
                joy.SetActive(false);
                PlayerPrefs.SetInt("joyType", 0);
            }
            
        }
    }

    public void StartTempScene(){
        SceneManager.LoadScene(tempScene);
    }

    public void PolicyPage(){
        Application.OpenURL(privacyPolicyLink);
    }

    public void StartTraaining()
    {
        SceneManager.LoadScene(trainingScene);
    }

    //public void SliderValue()
    //{
    //    var setHealth = Mathf.RoundToInt(setHealthValue.value / setHealthValue.maxValue) ;
    //    
    //}
    
}
