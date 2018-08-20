using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour {

    public GameObject m_despawnLocation;
    public GameObject m_citizen;

    [Range(0.0f,20.0f)]
    public float m_minSpawnTime;

    [Range(0.0f,10.0f)]
    public float m_randSpawnTimeOffset;

    private float m_nextSpawnTime;
    float m_spawnRadiusOffset;

    private void Start()
    {
        m_nextSpawnTime = Time.time + m_minSpawnTime + Random.Range(0, m_randSpawnTimeOffset);
        m_spawnRadiusOffset = GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update () {
		if(Time.time > m_nextSpawnTime)
        {
            m_nextSpawnTime = Time.time + m_minSpawnTime + Random.Range(0, m_randSpawnTimeOffset);
            Vector2 randOffset = (Random.insideUnitCircle * m_spawnRadiusOffset);
            Vector3 startPosition = new Vector3(transform.position.x + randOffset.x, transform.position.y, transform.position.z + randOffset.y);
            Instantiate(m_citizen, startPosition, transform.rotation);
        }
	}
}
