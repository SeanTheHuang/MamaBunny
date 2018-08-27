using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyOrderController : MonoBehaviour {

    private List<BunnyOrderSlot> m_orderSpots;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            m_orderSpots.Add(child.gameObject.GetComponent<BunnyOrderSlot>());
        }
    }

    public void MakeANewOrder()
    {
        for(int i = 0; i < m_orderSpots.Count; ++i)
        {
            if (!m_orderSpots[i].GetIsActive())
            {
                StartOrder(i);
                break;
            }
        }
    }

    void StartOrder(int orderNumber)
    {
        m_orderSpots[orderNumber].StartOrder();
    }
}
