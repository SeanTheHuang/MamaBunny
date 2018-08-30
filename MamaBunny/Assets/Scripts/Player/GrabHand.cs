using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabHand : MonoBehaviour {

    public TextMeshProUGUI m_text;
    public float m_range = 3.0f;
    Camera m_grabCam;
    public Inventory m_inventory;
    bool m_handLocked = false;
    public LayerMask m_pickupLayer;
    public HoldHand m_holdHand;

    PlayerControl m_playerControl;
    Transform m_hitTarget;

	// Use this for initialization
	void Start ()
    {
        m_playerControl = GetComponentInParent<PlayerControl>();
        m_grabCam = Camera.main;
        m_text.text = "";
       // m_inventory = GetComponent<Inventory>();
	}

    void Update()
    {
        if(m_handLocked )
        {
            return;
        }

        RaycastCheck();

        if(Input.GetKeyDown(KeyCode.E))
        {
            Grab();
        }
    }

    void RaycastCheck()
    {
        RaycastHit hit;
        Ray rayy = new Ray(transform.position, m_grabCam.transform.forward);

        if (Physics.SphereCast(rayy, 0.5f, out hit, m_range, m_pickupLayer))
        {
            if (m_text)
                m_text.text = "";

            // See if hit bunnycage
            BunnyPen pen = hit.transform.GetComponent<BunnyPen>();
            if (pen != null)
            {
                m_hitTarget = hit.transform;
                if (m_text)
                {
                    if(pen.m_penData.m_bunnyInside)
                        m_text.text = "Open";
                    else
                        m_text.text = "Close";
                }
                return;
            }

            // See if hit table
            GunTable table = hit.transform.GetComponent<GunTable>();
            if (table != null)
            {
                if (m_text)
                    m_text.text = "Interact";
            }

            // See if hit rabboid
            Rabboid rab = hit.transform.GetComponent<Rabboid>();
            if (rab != null && !rab.m_insidePen)
            {
                if (m_text)
                    m_text.text = "Grab";
            }

            // See if hit pickup
            PickUp pickUp = hit.transform.GetComponent<PickUp>();
            if (pickUp != null)
            {
                if (m_text)
                    m_text.text = pickUp.m_name;
            }
                m_hitTarget = hit.transform;
        }
        else
        {
            m_hitTarget = null;
            if (m_text)
                m_text.text = "";
        }
    }

    void Grab()
    {
        bool tryingToPutRabbitinCage = false;
        if (m_hitTarget)
        {
            BunnyPen bpen = m_hitTarget.GetComponent<BunnyPen>();
            if (m_holdHand.IsHolding() && bpen == null)
            {
                m_holdHand.Drop();
                return;
            }
            tryingToPutRabbitinCage = true;
        }

        if (!tryingToPutRabbitinCage && m_holdHand.IsHolding())
        {
            m_holdHand.Drop();
            return;
        }

        if (!m_hitTarget)
            return;

        GunTable table = m_hitTarget.GetComponent<GunTable>();
        if (table != null)
        {
            Debug.Log("table found");
            if (m_playerControl.IsGunActive())
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

        BunnyPen pen = m_hitTarget.GetComponent<BunnyPen>();
        if (pen != null)
        {
            RabboidResult rabboidResult = new RabboidResult();

            if(pen.m_penData.m_bunnyInside && m_holdHand.IsHolding())
            {
                return;
            }
            else if(!pen.m_penData.m_bunnyInside && !m_holdHand.IsHolding())
            {
                return;
            }

            if (m_holdHand.IsHoldingBunny())
            {
                // Put the rabbit in the cage in the players hand
                rabboidResult = m_holdHand.GetBunnyData();
                pen.PlayerInteract(true, rabboidResult);
                m_holdHand.DestroyItem();
            }
            else
            {
                // Pick up the caged rabbit
                pen.PlayerInteract(false, rabboidResult);
            }
        }

        Rabboid rab = m_hitTarget.GetComponent<Rabboid>();
        if (rab != null && !rab.m_insidePen)
        {
            m_holdHand.Hold(rab.transform);
            return;
        }

        PickUp pickUp = m_hitTarget.GetComponent<PickUp>();
        if (pickUp != null)
        {
            if (m_inventory.AddToInventory(pickUp))
            {
                //Debug.Log("sphere pickup");
                SoundEffectsPlayer.Instance.PlaySound("PickUp");
                Destroy(pickUp.gameObject);
                return;
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
