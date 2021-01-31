using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{

    public AudioMixer mixer1;
    public AudioMixer mixer2;

    public Slider mSlider;
    public Slider sSlider;

    

    void Start()
    {
        mSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);

        //toggle = GetComponent<Toggle>();

        
    }

    void Update()
    {
        
    }

    

    public void setVolMin()
    {
        mixer1.SetFloat("Sound", -80);
    }

    public void SetLevel(float sliderValue)
    {
        mixer1.SetFloat("Sound", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void setSfxMin(float sliderValue)
    {
        mixer2.SetFloat("Sound", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

}