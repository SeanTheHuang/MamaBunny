using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHand : MonoBehaviour {

    public float m_range = 3.0f;
    Camera m_grabCam;
    public Inventory m_inventory;
    bool m_handLocked = false;
    public LayerMask m_pickupLayer;
    public HoldHand m_holdHand;

    PlayerControl m_playerControl;

	// Use this for initialization
	void Start ()
    {
        m_playerControl = GetComponentInParent<PlayerControl>();
        m_grabCam = Camera.main;
       // m_inventory = GetComponent<Inventory>();
	}

    void Update()
    {
        if(m_handLocked )
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Grab();
        }
    }

    void Grab()
    {
        if (m_holdHand.IsHolding())
        {
            m_holdHand.Drop();
            return;
        }

        RaycastHit hit;
        Ray rayy = new Ray(transform.position, m_grabCam.transform.forward);

        if (Physics.SphereCast(rayy, 0.5f, out hit, m_range, m_pickupLayer)) 
        {
            PickUp pickUp = hit.transform.GetComponent<PickUp>();
            if(pickUp != null)
            {
                if(m_inventory.AddToInventory(pickUp))
                {
                    //Debug.Log("sphere pickup");
                    SoundEffectsPlayer.Instance.PlaySound("PickUp");
                    Destroy(pickUp.gameObject);
                    return;
                }
            }

            Rabboid rab = hit.transform.GetComponent<Rabboid>();
            if (rab != null) 
            {
                m_holdHand.Hold(rab.transform);
                return;
            }

            Debug.Log("trynagrab");
            GunTable table = hit.transform.GetComponent<GunTable>();
            if(table != null)
            {
                Debug.Log("table found");
                if(m_playerControl.IsGunActive())
                {//gun is active //place gun
                    m_playerControl.ActiveGun(false);
                    table.PlaceGun();
                }
                else
                {//gun is not active // take gun
                    m_playerControl.ActiveGun(true);
                    table.TakeGun();
                }
            }
        }

        //didnt hit anything//drop holditem
        


        /*if (Physics.Raycast(rayy, out hit, m_range)) 
        {
            PickUp pickup = hit.transform.GetComponent<PickUp>();
            if(pickup != null)
            {
                //Debug.Log("pick up " + pickup.name);
                if(m_inventory.AddToInventory(pickup))
                {
                   // Debug.Log("added" + pickup.name);
                    Destroy(pickup.gameObject);
                }
            }
        }*/
    }

    public void LockHand(bool _lock)
    {
        m_handLocked = _lock;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + m_grabCam.transform.forward * m_range);

            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawWireSphere(transform.position + m_grabCam.transform.forward * m_range, 0.5f);
        }
    }
}
