using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BunnyOrderController : MonoBehaviour {

    private BunnyOrderSlot[] m_orderSpots;

    public RabboidColour[] m_possibleOrderColours;
    public RabboidBodyPart[] m_possibleOrderBodies;
    public RabboidBodyPart[] m_possibleOrderHeads;

    public TextMeshProUGUI m_coinCounter;

    private void Start()
    {
        m_orderSpots = GetComponentsInChildren<BunnyOrderSlot>();
    }

    public void MakeANewOrder(GameObject customer)
    {
        for(int i = 0; i < m_orderSpots.Length; ++i)
        {
            if (!m_orderSpots[i].GetIsActive())
            {
                StartOrder(i, customer);
                customer.GetComponent<Customer>().SetOrderDestination(i);
                break;
            }
        }
    }

    void StartOrder(int orderNumber, GameObject customer)
    {
        m_orderSpots[orderNumber].GenerateOrder(customer, m_possibleOrderColours, m_possibleOrderBodies, m_possibleOrderHeads);
    }

    public void UpdateCoinCounter(uint coinCount)
    {
        m_coinCounter.text = "Coins: " + coinCount;
    }

    public void CancelAllOrders()
    {
        foreach(BunnyOrderSlot slot in m_orderSpots)
        {
            if(slot.m_customerOrder.m_isActive)
            {
                slot.CancelOrder();
            }
        }
    }
}
