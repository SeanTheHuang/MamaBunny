using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Camera m_fpsCam;
    public float m_range = 100.0f;

    public float m_timeBetweenShots = 0.2f;
    float lastShotTime = 0;

    public float m_damage = 10.0f;

    ParticleSystem m_shotPart;
    public GameObject impactParticle;

    bool m_gunLocked;

	void Start () {
        m_fpsCam = Camera.main;
        m_shotPart = GetComponentInChildren<ParticleSystem>();
        m_gunLocked = false;
	}

    // Update is called once per frame
    void Update()
    {
        if(m_gunLocked)
        {
            return;
        }

        if (Time.time - lastShotTime >= m_timeBetweenShots)
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
        m_shotPart.Play();
        RaycastHit hit;

        if (Physics.Raycast(transform.position, m_fpsCam.transform.forward, out hit, m_range))
        {
            GameObject go = Instantiate(impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(go, 1.0f);

            GunTarget gt = hit.transform.GetComponent<GunTarget>();
            Debug.Log(hit.transform.name);
            if (gt != null) 
            {
                gt.TakeHit(m_damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + m_fpsCam.transform.forward * m_range);
        }
    }

    public void LockGun(bool _lock)
    {
        m_gunLocked = _lock;
    }
}
