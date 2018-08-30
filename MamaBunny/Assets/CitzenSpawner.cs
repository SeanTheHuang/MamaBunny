using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitzenSpawner : MonoBehaviour {

    public List<Customer> m_customerList;

    private CustomerSpawner[] m_customerSpawners;

    private void Start()
    {
        m_customerSpawners = GetComponentsInChildren<CustomerSpawner>();
    }

    public void MakeCitizensRunInFear()
    {
        foreach(Customer customer in m_customerList)
        {
            customer.RunInPanic();
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        m_customerList.Remove(customer);
    }

    public void MakeCustomersSpawn(bool customersSpawn)
    {
        foreach(CustomerSpawner spawner in m_customerSpawners)
        {
            spawner.m_spawning = customersSpawn;
        }
    }
}
