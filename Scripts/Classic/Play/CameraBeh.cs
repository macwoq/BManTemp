using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBeh : MonoBehaviour
{

    public float shakeAmmount = 1;
    private Vector3 startingLocalPos;

    // Start is called before the first frame update
    
    public void Shake()
    {
        shakeAmmount = Mathf.Min(0.1f, shakeAmmount + 0.01f);

    }

    public void MediumShake()
    {
        shakeAmmount = Mathf.Min(0.15f, shakeAmmount + 0.015f);
    }

}
