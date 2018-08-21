using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserbySpawner : MonoBehaviour
{

    public GameObject m_despawnLocation;
    public GameObject m_passerby;

    [MinMaxRange(0.0f, 20.0f)]
    public RangedFloat m_spawnTime;

    [Range(0.0f, 10.0f)]
    public float m_spawnRadiusOffset;

    private float m_nextSpawnTime;

    private void Start()
    {
        m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_nextSpawnTime)
        {
            m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
            Vector2 randOffset = (Random.insideUnitCircle * m_spawnRadiusOffset);
            Vector3 startPosition = new Vector3(transform.position.x + randOffset.x, transform.position.y, transform.position.z + randOffset.y);
            GameObject passerby = Instantiate(m_passerby, startPosition, transform.rotation);
            Vector3 endPosition = new Vector3(m_despawnLocation.transform.position.x + randOffset.x, m_despawnLocation.transform.position.y, m_despawnLocation.transform.position.z + randOffset.y);
            passerby.GetComponent<Passerby>().SetTargetLocation(endPosition);
        }
    }
}
