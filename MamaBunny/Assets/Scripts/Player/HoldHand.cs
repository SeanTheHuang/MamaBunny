﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldHand : MonoBehaviour {

    Transform m_holdItem;
    bool m_gotRabboid = false;

   
	// Use this for initialization
	void Start () {
		
	}
	
    public bool IsHolding()
    {
        if(m_holdItem == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Hold(Transform _tr)
    {
        if (m_holdItem != null) 
        {
            Drop();
        }
        m_holdItem = _tr;
        m_holdItem.transform.position = transform.position;
        m_holdItem.GetComponent<Rigidbody>().isKinematic = true;
        m_holdItem.parent = transform;
        m_holdItem.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (m_holdItem.GetComponent<Rabboid>())
        {
            m_gotRabboid = true;
            m_holdItem.GetComponentInChildren<MeshCollider>().enabled = false;
        }
    }

    public void Drop()
    {
        if(m_gotRabboid == true)
        {
            m_holdItem.GetComponentInChildren<MeshCollider>().enabled = true;
            m_gotRabboid = false;
        }
        m_holdItem.GetComponent<Rigidbody>().isKinematic = false;
        m_holdItem.parent = null;
        m_holdItem.position = transform.position + transform.forward;
        m_holdItem.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        m_holdItem = null;
    }
}