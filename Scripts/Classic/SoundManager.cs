using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundManager : ScriptableObject
{
    // Start is called before the first frame update
    public AudioClip[] audioClips;

    public AudioClip GetRandomClip(){
        if(audioClips.Length == 0){
            return null;
        }
        return audioClips[Random.Range(0,audioClips.Length)];
    }
}
