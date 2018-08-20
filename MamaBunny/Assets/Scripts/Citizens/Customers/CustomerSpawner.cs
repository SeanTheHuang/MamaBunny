using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{

    List <Vector3> m_TravelLocations;
    public GameObject m_customer;

    [Range(0.0f, 20.0f)]
    public float m_minSpawnTime;

    [Range(0.0f, 10.0f)]
    public float m_randSpawnTimeOffset;

    [Range(0.0f, 10.0f)]
    public float m_spawnRadiusOffset;

    private float m_nextSpawnTime;

    private void Start()
    {
        m_nextSpawnTime = Time.time + m_minSpawnTime + Random.Range(0, m_randSpawnTimeOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_nextSpawnTime)
        {
            m_nextSpawnTime = Time.time + m_minSpawnTime + Random.Range(0, m_randSpawnTimeOffset);
            Vector2 randOffset = (Random.insideUnitCircle * m_spawnRadiusOffset);
            Vector3 startPosition = new Vector3(transform.position.x + randOffset.x, transform.position.y, transform.position.z + randOffset.y);
            GameObject customer = Instantiate(m_customer, startPosition, transform.rotation);

            //Vector3 endPosition = new Vector3(m_despawnLocation.transform.position.x + randOffset.x, m_despawnLocation.transform.position.y, m_despawnLocation.transform.position.z + randOffset.y);

            // First travel location is infront of the shop
            //m_TravelLocations.Add();

            // Second location is at the counter
        }
    }
}
