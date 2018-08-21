using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rabboid : MonoBehaviour {

    [Header("Spawn Body part points")]
    public Transform m_backPoint;
    public Transform m_mouthPoint;

    [Header("Default body parts")]
    public Transform m_defaultBack;
    public Transform m_defaultMouth;

    public void Initialize(RabboidResult _result)
    {
        // Set back
        if (_result.m_backPart)
            Instantiate(_result.m_backPart.m_bodyPrefab, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);
        else if (m_defaultBack)
            Instantiate(m_defaultBack, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);

        // Set mouth
        if (_result.m_mouthPart)
            Instantiate(_result.m_mouthPart.m_bodyPrefab, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);
        else if (m_defaultMouth)
            Instantiate(m_defaultMouth, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);

        // Set colour


        // Set size
        transform.localScale = transform.localScale * _result.m_size;


        // Output name
        Debug.Log("New Rabboid: " + _result.m_name);
    }

}
