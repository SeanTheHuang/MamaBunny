using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitzenSpawner : MonoBehaviour {

    public List<Customer> m_customerList;

    public void MakeCitizensRunInFear()
    {
        foreach(Customer customer in m_customerList)
        {
            customer.RunInPanic();
        }
    }
}
