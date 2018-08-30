using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGun : MonoBehaviour {

    public GameObject m_spawner;
    private CitzenSpawner m_citzenSpawner;
    public GameObject m_BunnyOrders;
    private BunnyOrderController m_orders;

    private void Start()
    {
        m_citzenSpawner = m_spawner.GetComponent<CitzenSpawner>();
        m_orders = m_BunnyOrders.GetComponent<BunnyOrderController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerControl control = other.GetComponent<PlayerControl>();
        if (control != null)
        {
            // Make everyone run
            if(control.IsGunActive())
            {
                m_citzenSpawner.MakeCitizensRunInFear();
                m_citzenSpawner.MakeCustomersSpawn(false);
                m_orders.CancelAllOrders();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerControl control = other.GetComponent<PlayerControl>();
        if (control != null)
        {
            // Make people spawn again
            if (control.IsGunActive())
            {
                m_citzenSpawner.MakeCustomersSpawn(true);
            }
        }
    }
}
