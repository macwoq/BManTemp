using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SondMixers : MonoBehaviour
{   
    public AudioSource soundS;
    public AudioSource musicS;

    public AudioMixer mixer1;
    public AudioMixer mixer2;
    public Slider sliderS;
    public Slider sliderM;
    public AudioClip[] musicClip;
    // Start is called before the first frame update
    void Start()
    {
        sliderM.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sliderS.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        PlayMusic();
    }

    private void Update() {        
        musicS.volume = sliderM.value;
        soundS.volume = sliderS.value;
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

    public void PlayMusic(){
        
        int musicClips = Random.Range(0,musicClip.Length);
        musicS.PlayOneShot(musicClip[musicClips]);
    }
}
