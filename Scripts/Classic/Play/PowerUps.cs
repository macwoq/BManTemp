using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public static PowerUps instance;

    public int prefabType;
    float speed = 3;
    


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Destroy(this.gameObject, 6f);
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player")
        {
            if(prefabType == 1)
            {
                if (Classic_PlayerBehaviour.instance.bulletLevel < 3)
                {
                    Classic_PlayerBehaviour.instance.bulletLevel++;
                    PlayerPrefs.SetInt("bullet", Classic_PlayerBehaviour.instance.bulletLevel);
                    GameManager.instance.PickUp(1);                    
                }else
                {
                    GameManager.instance.PickUp(2);
                }
            }
            if(prefabType == 0)
            {
                if (Classic_PlayerBehaviour.instance.bulletLevel > 1)
                {
                    Classic_PlayerBehaviour.instance.bulletLevel--;
                    PlayerPrefs.SetInt("bullet", Classic_PlayerBehaviour.instance.bulletLevel);
                    GameManager.instance.PickUp(0);                    
                }
                else
                {
                    GameManager.instance.PickUp(2);
                }
            }

            Destroy(this.gameObject);
        }
    }
}
