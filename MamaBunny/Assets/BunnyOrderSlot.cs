using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyOrderSlot : MonoBehaviour {

    public float m_orderTime; // How long the player has before the order is cancelled

    bool m_isActive;
    float m_startOfOrderTime;
    BunnyOrderController m_bunnyOrderController;

	// Use this for initialization
	void Start () {
        m_bunnyOrderController = transform.GetComponentInParent<BunnyOrderController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_isActive)
            UpdateOrder();
    }

    private void UpdateOrder()
    {
        // Check the player has not run out of time on the order
        if(m_startOfOrderTime < Time.time)
        {
            EndOrder();
        }
    }

    public void StartOrder()
    {
        m_isActive = true;
        m_startOfOrderTime = Time.time + m_orderTime;
    }

    void EndOrder()
    {
        m_isActive = false;
    }

    public bool GetIsActive()
    {
        return m_isActive;
    }
}
