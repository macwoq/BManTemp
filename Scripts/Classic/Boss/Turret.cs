using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public bool isShooting = false;
    [SerializeField] private GameObject laser;
    public int shootRate = 10;
    [SerializeField] private Transform shootPoint;

    public bool isTargeting;
    public float speed = 10f;
    private Vector3 lookAt;

    

    [SerializeField] private Transform player;
    
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        shootPoint.LookAt(player);
        if (isTargeting)
        {
            Targeting();
        }
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(4);
        isShooting = true;

        int timesShoot = 10;
        for (int i = 0; i < timesShoot; i++)
        {
            
            Instantiate(laser, shootPoint.transform.position, shootPoint.rotation);

            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0f);

        StartCoroutine(Shooting());
    }

    public void Targeting()
    {
        lookAt = player.position - transform.position;
        lookAt.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position * speed), Time.deltaTime);
    }

}
