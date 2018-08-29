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

    protected Vector3 m_startPos;
    protected float m_shakeStartTime = 0;
    protected bool m_shaking = false;
    public float m_shakeAmount = 0.03f;

	// Use this for initialization
	void Start ()
    {
        m_startPos = transform.position;
        StartCoroutine(SpawnAnimation());
	}

    public virtual void TakeHit(float _damage)
    {
        SoundEffectsPlayer.Instance.PlaySound("HitVeganFood");
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

        Transform t = Instantiate(m_spawnedItemPrefab, transform.position, transform.rotation);
        Rigidbody rb = t.GetComponent<Rigidbody>();
        float rStr = 20;
        float pStr = 3;
        if (rb)
        {
            rb.AddTorque(new Vector3(Random.Range(-rStr, rStr), Random.Range(-rStr, rStr), Random.Range(-rStr, rStr)), ForceMode.VelocityChange);
            rb.AddForce(new Vector3(Random.Range(-pStr, pStr), 0, Random.Range(-pStr, pStr)), ForceMode.VelocityChange);
        }

        m_dying = true;
        Destroy(gameObject);
    }

    protected IEnumerator Shake()
    {
        m_shaking = true;

        while (Time.time - m_shakeStartTime < 0.2f)
        {
            transform.position = m_startPos + (Random.onUnitSphere * m_shakeAmount);
            yield return null;
        }

        transform.position = m_startPos;
        m_shaking = false;
        yield return null;
    }

    IEnumerator SpawnAnimation()
    {
        float spawnTime = 0.3f;
        float targetScale = transform.localScale.x;
        float scaleRate = targetScale / spawnTime;
        transform.localScale = Vector3.zero;

        while (transform.localScale.x < targetScale)
        {
            float newScale = transform.localScale.x + scaleRate * Time.deltaTime;
            transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }

        transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
