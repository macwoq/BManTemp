using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotatePosition : MonoBehaviour
{

    public Vector3 rotateAxis;
    public Vector3 positionMove;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(-270, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
