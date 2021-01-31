using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : Singleton<SoundsManager>
{
    public static SoundsManager instance;

    public SoundManager[] effects;

    private Dictionary<string, SoundManager> _effectDictionary;
    private AudioListener _listaner;

    
    private void Awake() {
        _effectDictionary = new Dictionary<string, SoundManager>();
        foreach(var effect in effects){
            Debug.LogFormat("registered {0}", effect.name);
            _effectDictionary[effect.name] = effect;
        }
    }
    // Start is called before the first frame update
    public void PlayEffect(string effectName)
    {
        if(_listaner == null){
            _listaner = FindObjectOfType<AudioListener>();
        }

        PlayEffect(effectName);//, _listaner.transform.position);
    }

    public void PlayEffect(string effectName, Vector3 worldPosition){
        if(_effectDictionary.ContainsKey(effectName) == false){
            Debug.LogWarning("{0}");
            return;
        }

        var clip = _effectDictionary[effectName].GetRandomClip();

        if(clip == null){
            Debug.LogWarning("{0}");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, worldPosition);
    }

}
