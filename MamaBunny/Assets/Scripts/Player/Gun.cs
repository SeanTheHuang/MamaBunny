using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class Gun : MonoBehaviour
{
    Camera m_fpsCam;
    public float m_range = 100.0f;

    public float m_damage = 10;
    public float m_timeBetweenShots = 0.2f;
    float lastShotTime = 0;
    public Transform m_bulletPrefab;
    public Transform m_firePoint;

    public PlayerCameraController m_cameraController;
    public ParticleSystem m_shotPart, m_sparkPart;
    public LayerMask m_gunHitLayers;
    public GameObject impactParticle;
    bool m_gunLocked;
    AudioSource m_source;
    Animation m_anim;

	void Start () {
        m_fpsCam = Camera.main;
        m_source = GetComponent<AudioSource>();
        m_anim = GetComponent<Animation>();
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
        m_cameraController.ApplyScreenShake(0.15f, 0.03f);
        m_shotPart.Play();
        m_sparkPart.Play();
        m_anim.Stop();
        m_anim.Play();
        m_source.PlayOneShot(m_source.clip);
        Transform bullet = Instantiate(m_bulletPrefab, m_firePoint.position, m_firePoint.rotation);
        bullet.GetComponent<Bullet>().Initialize(transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(m_firePoint.position, m_firePoint.forward, out hit, m_range, m_gunHitLayers))
        {
            GameObject go = Instantiate(impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(go, 1.0f);

            GunTarget gt = hit.transform.GetComponent<GunTarget>();
            Debug.Log("hit: " + hit.transform.name);
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
            Gizmos.DrawLine(m_firePoint.position, m_firePoint.position + m_fpsCam.transform.forward * m_range);
        }
    }

    public void LockGun(bool _lock)
    {
        m_gunLocked = _lock;
    }
}
