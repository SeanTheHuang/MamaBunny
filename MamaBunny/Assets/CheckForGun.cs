using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGun : MonoBehaviour {

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
