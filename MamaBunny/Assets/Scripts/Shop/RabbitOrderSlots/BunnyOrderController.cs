﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyOrderController : MonoBehaviour {

    private BunnyOrderSlot[] m_orderSpots;

    public RabboidColour[] m_possibleOrderColours;
    public RabboidBodyPart[] m_possibleOrderBodies;
    public RabboidBodyPart[] m_possibleOrderHeads;

    private void Start()
    {
        m_orderSpots = GetComponentsInChildren<BunnyOrderSlot>();
        Debug.Log(m_orderSpots.Length);
    }

    public void MakeANewOrder(GameObject customer)
    {
        for(int i = 0; i < m_orderSpots.Length; ++i)
        {
            if (!m_orderSpots[i].GetIsActive())
            {
                StartOrder(i, customer);
                break;
            }
        }
    }

    void StartOrder(int orderNumber, GameObject customer)
    {
        m_orderSpots[orderNumber].GenerateOrder(customer, m_possibleOrderColours, m_possibleOrderBodies, m_possibleOrderHeads);
    }
}