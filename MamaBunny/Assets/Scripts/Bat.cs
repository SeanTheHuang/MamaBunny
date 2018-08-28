using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : GunTarget {

    bool m_dead = false;
    Rigidbody m_rgbd;
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360.0f), 0));
        m_rgbd = GetComponent<Rigidbody>();
        m_startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(m_dead)
        {
            
        }
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
