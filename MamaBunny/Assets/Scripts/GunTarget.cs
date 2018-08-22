using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTarget : MonoBehaviour
{
    [HideInInspector]
    public ItemSpawner m_parent;
    [HideInInspector]
    public int m_index;

    public Transform m_spawnedItemPrefab;
    public float m_health = 50.0f;
    bool m_dying = false;

    Vector3 m_startPos;
    float m_shakeStartTime = 0;
    bool m_shaking = false;

	// Use this for initialization
	void Start ()
    {
        m_startPos = transform.position;
	}

    public void TakeHit(float _damage)
    {
        m_health -= _damage;

        m_shakeStartTime = Time.time;
        if (m_shaking == false)
        {
            StartCoroutine(Shake());
        }

        if(m_health <= 0)
        {
            m_health = 0;
            if (m_dying == false)
            {
                Death();
            }
        }
    }

    void Death()
    {
        if (m_parent)
            m_parent.OnItemsDeath(m_index);

        Instantiate(m_spawnedItemPrefab, transform.position, transform.rotation);
        m_dying = true;
        Destroy(gameObject);
    }

    IEnumerator Shake()
    {
        m_shaking = true;

        while (Time.time - m_shakeStartTime < 0.2f)
        {
            transform.position = m_startPos + (Random.onUnitSphere * 0.03f);
            yield return null;
        }

        transform.position = m_startPos;
        m_shaking = false;
        yield return null;
    }
}
