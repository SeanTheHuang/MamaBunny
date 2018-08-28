using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject m_customer;
    public GameObject m_orderingCustomer;
    public CustomerCounter m_customerCounter;

    public List<GameObject> m_characterModels;

    [MinMaxRange(0.0f, 20.0f)]
    public RangedFloat m_spawnTime;

    [Range(0.0f, 10.0f)]
    public float m_spawnRadiusOffset;

    private float m_nextSpawnTime;

    List<Vector3> m_travelLocations = new List<Vector3>();

    private void Start()
    {
        m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);

        // First travel location is infront of the shop
        m_travelLocations.Add(transform.parent.position);
        // Second location is at the counter
        m_travelLocations.Add(transform.parent.Find("ShopEntryPoint").position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_nextSpawnTime)
        {
            m_nextSpawnTime = Time.time + Random.Range(m_spawnTime.minValue, m_spawnTime.maxValue);
            Vector2 randOffset = (Random.insideUnitCircle * m_spawnRadiusOffset);
            Vector3 startPosition = new Vector3(transform.position.x + randOffset.x, transform.position.y, transform.position.z + randOffset.y);
            GameObject customer = Instantiate(m_customer, startPosition, transform.rotation);
            customer.GetComponent<Customer>().SetTravelLocations(m_travelLocations);

            GameObject model = Instantiate(m_characterModels[Random.Range(0, m_characterModels.Count)], customer.transform.position, customer.transform.rotation);
            model.transform.parent = customer.transform;

            ++m_customerCounter.m_cusomterCounter;
            if (m_customerCounter.m_cusomterCounter > 16)
            {
                m_customerCounter.m_cusomterCounter = 0;
                customer.GetComponent<Customer>().m_DemandingCustomer = true;
            }
        }
    }
}
