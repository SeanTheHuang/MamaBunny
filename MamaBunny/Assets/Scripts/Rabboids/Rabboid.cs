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

    public Transform[] m_otherBodyParts;

    public void Initialize(RabboidResult _result)
    {
        Transform newBack = null, newMouth = null;

        // Set back
        if (_result.m_backPart)
            newBack = Instantiate(_result.m_backPart.m_bodyPrefab, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);
        else if (m_defaultBack)
            newBack = Instantiate(m_defaultBack, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);

        // Set mouth
        if (_result.m_mouthPart)
            newMouth = Instantiate(_result.m_mouthPart.m_bodyPrefab, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);
        else if (m_defaultMouth)
            newMouth = Instantiate(m_defaultMouth, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);

        // Set colour
        if (_result.m_resultColour)
            SetColours(_result.m_resultColour, newBack, newMouth);

        // Set size
        transform.localScale = transform.localScale * _result.m_size;


        // Output name
        Debug.Log("New Rabboid: " + _result.m_name);
    }

    void SetColours(RabboidColour _colours, Transform _newBack, Transform _newMouth)
    {
        // Set colours - body
        Material newBodyMat = new Material(GetComponent<Renderer>().sharedMaterial);
        newBodyMat.color = _colours.m_color;
        GetComponent<Renderer>().sharedMaterial = newBodyMat;

        // Set colours - extra features
        foreach (Transform t in m_otherBodyParts)
        {
            newBodyMat = new Material(t.GetComponent<Renderer>().sharedMaterial);
            newBodyMat.color = _colours.m_color;
            t.GetComponent<Renderer>().sharedMaterial = newBodyMat;
        }

        if (_newBack)
        {
            Renderer[] rendList = _newBack.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rendList)
            {
                newBodyMat = new Material(r.sharedMaterial);
                newBodyMat.color = _colours.m_color;
                r.sharedMaterial = newBodyMat;
            }
        }

        if (_newMouth)
        {
            Renderer[] rendList = _newMouth.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rendList)
            {
                newBodyMat = new Material(r.sharedMaterial);
                newBodyMat.color = _colours.m_color;
                r.sharedMaterial = newBodyMat;
            }
        }
    }

}
