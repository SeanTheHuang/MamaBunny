using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHand : MonoBehaviour {

    public float m_range = 3.0f;
    Camera m_grabCam;
    public Inventory m_inventory;

	// Use this for initialization
	void Start () {
        m_grabCam = Camera.main;
       // m_inventory = GetComponent<Inventory>();
	}

    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Grab();
        }
    }

    void Grab()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, m_grabCam.transform.forward, out hit, m_range)) 
        {
            PickUp pickup = hit.transform.GetComponent<PickUp>();
            if(pickup != null)
            {
                Debug.Log("pick up " + pickup.name);
                if(m_inventory.AddToInventory(pickup))
                {
                    Destroy(pickup);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + m_grabCam.transform.forward * m_range);
        }
    }
}
