using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : GunTarget {

    bool m_dead = false;
    Rigidbody m_rgbd;

    public AudioClip m_screamClip;
    AudioSource m_audioSource;
	void Start ()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360.0f), 0));
        m_rgbd = GetComponent<Rigidbody>();
        m_startPos = transform.position;
        m_audioSource = GetComponent<AudioSource>();
	}


    public override void TakeHit(float _damage)
    {
        if (m_dead)
            return;

        m_health -= _damage;

        m_shakeStartTime = Time.time;
        if(m_shaking == false)
        {
            StartCoroutine(Shake());
        }

        if(m_health < 1)
        {
            OnDeath();
            return;
        }
    }

    void OnDeath()
    {
        m_audioSource.Stop();
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.clip = m_screamClip;
        m_audioSource.Play();
        m_dead = true;
        m_rgbd.useGravity = true;
    }
    
    void SpawnItemsAndDestroy()
    {
        Instantiate(m_spawnedItemPrefab, transform.position + Vector3.up, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_dead)
        {
            SpawnItemsAndDestroy();
        }
    }
}
