using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGun : MonoBehaviour {

    public GameObject m_spawner;
    private CitzenSpawner m_citzenSpawner;

    private void Start()
    {
        m_citzenSpawner = m_spawner.GetComponent<CitzenSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerControl control = other.GetComponent<PlayerControl>();
        if (control != null)
        {
            // Make everyone run
            if(control.IsGunActive())
            {
                
            }
        }
    }
}
