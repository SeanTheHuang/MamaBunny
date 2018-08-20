﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject m_customer;

    [Range(0.0f, 20.0f)]
    public float m_minSpawnTime;

    [Range(0.0f, 10.0f)]
    public float m_randSpawnTimeOffset;

    [Range(0.0f, 10.0f)]
    public float m_spawnRadiusOffset;

    private float m_nextSpawnTime;

    List<Vector3> m_travelLocations = new List<Vector3>();

    private void Start()
    {
        m_nextSpawnTime = Time.time + m_minSpawnTime + Random.Range(0, m_randSpawnTimeOffset);

        // First travel location is infront of the shop
        m_travelLocations.Add(transform.parent.position);
        // Second location is at the counter
        m_travelLocations.Add(transform.parent.Find("CustomerEndPoint").position);
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
            customer.GetComponent<Customer>().SetTravelLocations(m_travelLocations);
        }
    }
}
