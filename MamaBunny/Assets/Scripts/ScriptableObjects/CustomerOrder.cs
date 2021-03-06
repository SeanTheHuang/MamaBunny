﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Customer/CustomerOrder")]
public class CustomerOrder : ScriptableObject
{
    public float m_size;
    public RabboidColour m_colour;
    public RabboidBodyPart m_mouthPart;
    public RabboidBodyPart m_backPart;
    public bool m_isActive;


    public ModelType m_modelType;

    public List<GameObject> m_models;

    private void OnEnable()
    {
       ResetVariables();
    }

    public void ResetVariables()
    {
        m_size = RabboidCalculator.SMALL_SIZE;
        m_colour = null;
        m_mouthPart = null;
        m_backPart = null;
        m_isActive = false;
    }
}
