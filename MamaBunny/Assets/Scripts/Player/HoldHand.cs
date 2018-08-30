using System.Collections;
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

    public bool IsHoldingBunny()
    {
        if (!m_gotRabboid)
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

        Rabboid rab = _tr.GetComponent<Rabboid>();

        m_holdItem = _tr;
        m_holdItem.transform.position = transform.position;
        if (rab)
            m_holdItem.position += transform.forward * rab.RabboidStats.m_size * 0.3f;
        m_holdItem.GetComponent<Rigidbody>().isKinematic = true;
        m_holdItem.parent = transform;
        m_holdItem.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (rab)
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
        Transform camtr = Camera.main.transform;
        m_holdItem.GetComponent<Rigidbody>().AddForce(camtr.forward.normalized * 5, ForceMode.Impulse);
        m_holdItem = null;
    }

    public void DestroyItem()
    {
        m_gotRabboid = false;
        foreach (Transform child in m_holdItem)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(m_holdItem.gameObject);
    }

    public RabboidResult GetBunnyData()
    {
        RabboidResult rabboidResult = new RabboidResult();
        if (m_gotRabboid == true)
        {
            rabboidResult = m_holdItem.GetComponent<Rabboid>().RabboidStats;
        }
        return rabboidResult;
    }
}
