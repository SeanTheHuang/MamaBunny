using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Customer/CustomerOrder")]
public class CustomerOrder : ScriptableObject
{
    public float m_size;
    public Color m_colour;
    public RabboidBodyPart m_mouthPart;
    public RabboidBodyPart m_backPart;
    public GameObject m_customer;
    public bool m_isActive;

    private void Awake()
    {
        ResetVariables();
    }

    public void ResetVariables()
    {
        m_size = 0;
        m_colour = Color.white;
        m_mouthPart = null;
        m_backPart = null;
        m_isActive = false;
    }
}
