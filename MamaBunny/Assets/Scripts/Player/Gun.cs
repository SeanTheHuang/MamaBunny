using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Camera m_fpsCam;
    public float m_range = 100.0f;

    public float timeBetweenShots = 0.2f;
    float lastShotTime = 0;

    ParticleSystem m_shotPart;
	void Start () {
        m_fpsCam = Camera.main;
        m_shotPart = GetComponentInChildren<ParticleSystem>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastShotTime >= timeBetweenShots)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }
	
    void Shoot()
    {
        Debug.Log("shot");
        m_shotPart.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_fpsCam.transform.forward, out hit, m_range))
        {
            
        }
    }
}
