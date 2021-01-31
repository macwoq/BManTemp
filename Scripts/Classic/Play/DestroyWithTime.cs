using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime : MonoBehaviour
{

    public float timeToDestroy = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

}
