using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : GunTarget {

    bool m_dead = false;
    Rigidbody m_rgbd;

    public Transform m_puffParticles;
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

        SoundEffectsPlayer.Instance.PlaySound("Ding");
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
        SoundEffectsPlayer.Instance.PlaySound("BatDie");
        m_dead = true;
        m_rgbd.useGravity = true;
    }
    
    void SpawnItemsAndDestroy()
    {
        Instantiate(m_spawnedItemPrefab, transform.position + Vector3.up, transform.rotation);
        Instantiate(m_puffParticles, transform.position, Quaternion.identity);
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
