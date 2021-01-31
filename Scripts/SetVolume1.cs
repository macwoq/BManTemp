using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume1 : MonoBehaviour
{
    public AudioSource mcMixer;

    // Start is called before the first frame update
    void Start()
    {
        mcMixer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKey(KeyCode.M))
        {
            stopMusic();
        }
    }

    public void stopMusic()
    {
        mcMixer.Stop();
    }
}
